using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Maestro.Models;
using Maestro.Services.Data;

namespace Maestro.Services.Community
{
	public class CommunityUploadService
	{
		private static readonly Logger Logger = Logger.GetLogger<CommunityUploadService>();

		private const int MIN_NAME_LENGTH = 3;

		private const int MIN_TRANSCRIBER_LENGTH = 2;

		private const int MIN_NOTE_COUNT = 10;

		private const long MIN_DURATION_MS = 15000L;

		private readonly CommunityApiClient _apiClient;

		private readonly CommunityService _communityService;

		private readonly UploadRateLimiter _rateLimiter;

		private readonly SongStorage _songStorage;

		public event EventHandler<UploadProgressEventArgs> UploadProgressChanged;

		public CommunityUploadService(CommunityApiClient apiClient, CommunityService communityService, UploadRateLimiter rateLimiter, SongStorage songStorage)
		{
			_apiClient = apiClient;
			_communityService = communityService;
			_rateLimiter = rateLimiter;
			_songStorage = songStorage;
		}

		public int GetRemainingUploads()
		{
			return _rateLimiter.GetRemainingUploads();
		}

		public UploadValidationResult ValidateSong(Song song)
		{
			UploadValidationResult result = new UploadValidationResult();
			if (string.IsNullOrWhiteSpace(song.Name))
			{
				result.NameValid = false;
				result.NameError = "Song name is required";
			}
			else if (song.Name.Trim().Length < 3)
			{
				result.NameValid = false;
				result.NameError = $"Song name must be at least {3} characters";
			}
			else
			{
				result.NameValid = true;
			}
			if (string.IsNullOrWhiteSpace(song.Transcriber))
			{
				result.TranscriberValid = false;
				result.TranscriberError = "Transcriber name is required";
			}
			else if (song.Transcriber.Trim().Length < 2)
			{
				result.TranscriberValid = false;
				result.TranscriberError = $"Transcriber must be at least {2} characters";
			}
			else
			{
				result.TranscriberValid = true;
			}
			if (!Enum.IsDefined(typeof(InstrumentType), song.Instrument))
			{
				result.InstrumentValid = false;
				result.InstrumentError = "Invalid instrument";
			}
			else
			{
				result.InstrumentValid = true;
			}
			int noteCount = GetNoteCount(song);
			if (noteCount < 10)
			{
				result.NotesValid = false;
				result.NotesError = $"Song must have at least {10} notes (has {noteCount})";
			}
			else
			{
				result.NotesValid = true;
			}
			long durationMs = song.DurationMs;
			if (durationMs < 15000)
			{
				result.DurationValid = false;
				result.DurationError = $"Song must be at least {15L}s long (is {(double)durationMs / 1000.0:0.#}s)";
			}
			else
			{
				result.DurationValid = true;
			}
			if (_communityService.HasManifest && string.IsNullOrEmpty(song.CommunityId))
			{
				CommunitySong existingSong = _communityService.GetAvailableSongs().FirstOrDefault((CommunitySong s) => s.Name.Equals(song.Name, StringComparison.OrdinalIgnoreCase) && s.Artist.Equals(song.Artist, StringComparison.OrdinalIgnoreCase) && s.Instrument.Equals(song.Instrument.ToString(), StringComparison.OrdinalIgnoreCase));
				if (existingSong != null)
				{
					result.IsDuplicate = true;
					result.DuplicateError = "A song with this name, artist, and instrument already exists (by " + existingSong.Transcriber + ")";
				}
			}
			if (!_rateLimiter.CanUpload())
			{
				result.RateLimitExceeded = true;
				result.RateLimitError = "Daily upload limit reached (3 per day)";
			}
			return result;
		}

		public async Task<UploadResponse> UploadSongAsync(Song song, CancellationToken cancellationToken = default(CancellationToken))
		{
			UploadValidationResult validation = ValidateSong(song);
			if (!validation.IsValid)
			{
				string error = GetFirstValidationError(validation);
				return new UploadResponse
				{
					Success = false,
					Error = error
				};
			}
			try
			{
				RaiseProgress(UploadState.Uploading, 0, "Preparing upload...");
				if (song.Notes == null || song.Notes.Count == 0)
				{
					return new UploadResponse
					{
						Success = false,
						Error = "Song has no notes data. Only songs with note data can be uploaded."
					};
				}
				RaiseProgress(UploadState.Uploading, 30, "Uploading to server...");
				UploadResponse response = await _apiClient.UploadSongAsync(song, cancellationToken);
				if (response.Success)
				{
					_rateLimiter.RecordUpload();
					song.IsUploaded = true;
					if (!string.IsNullOrEmpty(response.SongId))
					{
						song.CommunityId = response.SongId;
					}
					_songStorage.SaveSong(song);
					RaiseProgress(UploadState.Completed, 100, "Upload complete!");
					Logger.Info("Successfully uploaded song: " + song.Name + " (ID: " + response.SongId + ")");
				}
				else
				{
					RaiseProgress(UploadState.Failed, 0, response.Error ?? "Upload failed");
					Logger.Warn("Upload failed for " + song.Name + ": " + response.Error);
				}
				return response;
			}
			catch (OperationCanceledException)
			{
				RaiseProgress(UploadState.Cancelled, 0, "Upload cancelled");
				throw;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Upload failed for " + song.Name);
				RaiseProgress(UploadState.Failed, 0, ex.Message);
				return new UploadResponse
				{
					Success = false,
					Error = ex.Message
				};
			}
		}

		private int GetNoteCount(Song song)
		{
			if (song.Notes != null && song.Notes.Count > 0)
			{
				return song.Notes.Count((string n) => !n.StartsWith("R:"));
			}
			return song.Commands.Count((SongCommand c) => c.Type == CommandType.KeyDown && (int)c.Key != 96 && (int)c.Key != 105);
		}

		private string GetFirstValidationError(UploadValidationResult validation)
		{
			if (!validation.NameValid)
			{
				return validation.NameError;
			}
			if (!validation.TranscriberValid)
			{
				return validation.TranscriberError;
			}
			if (!validation.InstrumentValid)
			{
				return validation.InstrumentError;
			}
			if (!validation.NotesValid)
			{
				return validation.NotesError;
			}
			if (!validation.DurationValid)
			{
				return validation.DurationError;
			}
			if (validation.IsDuplicate)
			{
				return validation.DuplicateError;
			}
			if (validation.RateLimitExceeded)
			{
				return validation.RateLimitError;
			}
			return "Unknown validation error";
		}

		private void RaiseProgress(UploadState state, int progress, string message)
		{
			this.UploadProgressChanged?.Invoke(this, new UploadProgressEventArgs(state, progress, message));
		}
	}
}
