using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Shared;

namespace Microsoft.AspNetCore.SignalR.Client
{
	internal static class HubConnectionExtensions
	{
		private static IDisposable On(this HubConnection hubConnection, string methodName, Type[] parameterTypes, Action<object[]> handler)
		{
			return hubConnection.On(methodName, parameterTypes, delegate(object[] parameters, object state)
			{
				((Action<object[]>)state)(parameters);
				return Task.CompletedTask;
			}, handler);
		}

		public static IDisposable On(this HubConnection hubConnection, string methodName, Action handler)
		{
			Action handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, Type.EmptyTypes, delegate
			{
				handler2();
			});
		}

		public static IDisposable On<T1>(this HubConnection hubConnection, string methodName, Action<T1> handler)
		{
			Action<T1> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[1] { typeof(T1) }, delegate(object[] args)
			{
				handler2((T1)args[0]);
			});
		}

		public static IDisposable On<T1, T2>(this HubConnection hubConnection, string methodName, Action<T1, T2> handler)
		{
			Action<T1, T2> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[2]
			{
				typeof(T1),
				typeof(T2)
			}, delegate(object[] args)
			{
				handler2((T1)args[0], (T2)args[1]);
			});
		}

		public static IDisposable On<T1, T2, T3>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3> handler)
		{
			Action<T1, T2, T3> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[3]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3)
			}, delegate(object[] args)
			{
				handler2((T1)args[0], (T2)args[1], (T3)args[2]);
			});
		}

		public static IDisposable On<T1, T2, T3, T4>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3, T4> handler)
		{
			Action<T1, T2, T3, T4> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[4]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4)
			}, delegate(object[] args)
			{
				handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
			});
		}

		public static IDisposable On<T1, T2, T3, T4, T5>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3, T4, T5> handler)
		{
			Action<T1, T2, T3, T4, T5> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[5]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5)
			}, delegate(object[] args)
			{
				handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4]);
			});
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3, T4, T5, T6> handler)
		{
			Action<T1, T2, T3, T4, T5, T6> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[6]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6)
			}, delegate(object[] args)
			{
				handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5]);
			});
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6, T7>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3, T4, T5, T6, T7> handler)
		{
			Action<T1, T2, T3, T4, T5, T6, T7> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[7]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7)
			}, delegate(object[] args)
			{
				handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6]);
			});
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6, T7, T8>(this HubConnection hubConnection, string methodName, Action<T1, T2, T3, T4, T5, T6, T7, T8> handler)
		{
			Action<T1, T2, T3, T4, T5, T6, T7, T8> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[8]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8)
			}, delegate(object[] args)
			{
				handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6], (T8)args[7]);
			});
		}

		public static IDisposable On(this HubConnection hubConnection, string methodName, Type[] parameterTypes, Func<object?[], Task> handler)
		{
			return hubConnection.On(methodName, parameterTypes, (object[] parameters, object state) => ((Func<object[], Task>)state)(parameters), handler);
		}

		public static IDisposable On(this HubConnection hubConnection, string methodName, Func<Task> handler)
		{
			Func<Task> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, Type.EmptyTypes, (object[] args) => handler2());
		}

		public static IDisposable On<T1>(this HubConnection hubConnection, string methodName, Func<T1, Task> handler)
		{
			Func<T1, Task> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[1] { typeof(T1) }, (object[] args) => handler2((T1)args[0]));
		}

		public static IDisposable On<T1, T2>(this HubConnection hubConnection, string methodName, Func<T1, T2, Task> handler)
		{
			Func<T1, T2, Task> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[2]
			{
				typeof(T1),
				typeof(T2)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1]));
		}

		public static IDisposable On<T1, T2, T3>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, Task> handler)
		{
			Func<T1, T2, T3, Task> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[3]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2]));
		}

		public static IDisposable On<T1, T2, T3, T4>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, Task> handler)
		{
			Func<T1, T2, T3, T4, Task> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[4]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]));
		}

		public static IDisposable On<T1, T2, T3, T4, T5>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, Task> handler)
		{
			Func<T1, T2, T3, T4, T5, Task> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[5]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4]));
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, Task> handler)
		{
			Func<T1, T2, T3, T4, T5, T6, Task> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[6]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5]));
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6, T7>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, Task> handler)
		{
			Func<T1, T2, T3, T4, T5, T6, T7, Task> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[7]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6]));
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6, T7, T8>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> handler)
		{
			Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[8]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6], (T8)args[7]));
		}

		public static Task InvokeAsync(this HubConnection hubConnection, string methodName, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync(methodName, Array.Empty<object>(), cancellationToken);
		}

		public static Task InvokeAsync(this HubConnection hubConnection, string methodName, object? arg1, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync(methodName, new object[1] { arg1 }, cancellationToken);
		}

		public static Task InvokeAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync(methodName, new object[2] { arg1, arg2 }, cancellationToken);
		}

		public static Task InvokeAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync(methodName, new object[3] { arg1, arg2, arg3 }, cancellationToken);
		}

		public static Task InvokeAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync(methodName, new object[4] { arg1, arg2, arg3, arg4 }, cancellationToken);
		}

		public static Task InvokeAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync(methodName, new object[5] { arg1, arg2, arg3, arg4, arg5 }, cancellationToken);
		}

		public static Task InvokeAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync(methodName, new object[6] { arg1, arg2, arg3, arg4, arg5, arg6 }, cancellationToken);
		}

		public static Task InvokeAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync(methodName, new object[7] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 }, cancellationToken);
		}

		public static Task InvokeAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync(methodName, new object[8] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 }, cancellationToken);
		}

		public static Task InvokeAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync(methodName, new object[9] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 }, cancellationToken);
		}

		public static Task InvokeAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync(methodName, new object[10] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 }, cancellationToken);
		}

		public static Task InvokeCoreAsync(this HubConnection hubConnection, string methodName, object?[] args, CancellationToken cancellationToken = default(CancellationToken))
		{
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.InvokeCoreAsync(methodName, typeof(object), args, cancellationToken);
		}

		public static Task<TResult> InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync<TResult>(methodName, Array.Empty<object>(), cancellationToken);
		}

		public static Task<TResult> InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync<TResult>(methodName, new object[1] { arg1 }, cancellationToken);
		}

		public static Task<TResult> InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync<TResult>(methodName, new object[2] { arg1, arg2 }, cancellationToken);
		}

		public static Task<TResult> InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync<TResult>(methodName, new object[3] { arg1, arg2, arg3 }, cancellationToken);
		}

		public static Task<TResult> InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync<TResult>(methodName, new object[4] { arg1, arg2, arg3, arg4 }, cancellationToken);
		}

		public static Task<TResult> InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync<TResult>(methodName, new object[5] { arg1, arg2, arg3, arg4, arg5 }, cancellationToken);
		}

		public static Task<TResult> InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync<TResult>(methodName, new object[6] { arg1, arg2, arg3, arg4, arg5, arg6 }, cancellationToken);
		}

		public static Task<TResult> InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync<TResult>(methodName, new object[7] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 }, cancellationToken);
		}

		public static Task<TResult> InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync<TResult>(methodName, new object[8] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 }, cancellationToken);
		}

		public static Task<TResult> InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync<TResult>(methodName, new object[9] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 }, cancellationToken);
		}

		public static Task<TResult> InvokeAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.InvokeCoreAsync<TResult>(methodName, new object[10] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 }, cancellationToken);
		}

		public static async Task<TResult> InvokeCoreAsync<TResult>(this HubConnection hubConnection, string methodName, object?[] args, CancellationToken cancellationToken = default(CancellationToken))
		{
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return (TResult)(await hubConnection.InvokeCoreAsync(methodName, typeof(TResult), args, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
		}

		private static IDisposable On<TResult>(this HubConnection hubConnection, string methodName, Type[] parameterTypes, Func<object[], TResult> handler)
		{
			return hubConnection.On(methodName, parameterTypes, (object[] parameters, object state) => Task.FromResult((object)((Func<object[], TResult>)state)(parameters)), handler);
		}

		public static IDisposable On<TResult>(this HubConnection hubConnection, string methodName, Type[] parameterTypes, Func<object?[], Task<TResult>> handler)
		{
			return hubConnection.On(methodName, parameterTypes, async (object[] parameters, object state) => await ((Func<object[], Task<TResult>>)state)(parameters).ConfigureAwait(continueOnCapturedContext: false), handler);
		}

		public static IDisposable On<TResult>(this HubConnection hubConnection, string methodName, Func<Task<TResult>> handler)
		{
			Func<Task<TResult>> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, Type.EmptyTypes, (object[] args) => handler2());
		}

		public static IDisposable On<TResult>(this HubConnection hubConnection, string methodName, Func<TResult> handler)
		{
			Func<TResult> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, Type.EmptyTypes, (object[] args) => handler2());
		}

		public static IDisposable On<T1, TResult>(this HubConnection hubConnection, string methodName, Func<T1, TResult> handler)
		{
			Func<T1, TResult> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[1] { typeof(T1) }, (object[] args) => handler2((T1)args[0]));
		}

		public static IDisposable On<T1, T2, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, TResult> handler)
		{
			Func<T1, T2, TResult> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[2]
			{
				typeof(T1),
				typeof(T2)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1]));
		}

		public static IDisposable On<T1, T2, T3, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, TResult> handler)
		{
			Func<T1, T2, T3, TResult> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[3]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2]));
		}

		public static IDisposable On<T1, T2, T3, T4, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, TResult> handler)
		{
			Func<T1, T2, T3, T4, TResult> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[4]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]));
		}

		public static IDisposable On<T1, T2, T3, T4, T5, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, TResult> handler)
		{
			Func<T1, T2, T3, T4, T5, TResult> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[5]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4]));
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, TResult> handler)
		{
			Func<T1, T2, T3, T4, T5, T6, TResult> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[6]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5]));
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6, T7, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, TResult> handler)
		{
			Func<T1, T2, T3, T4, T5, T6, T7, TResult> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[7]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6]));
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> handler)
		{
			Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[8]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6], (T8)args[7]));
		}

		public static IDisposable On<T1, TResult>(this HubConnection hubConnection, string methodName, Func<T1, Task<TResult>> handler)
		{
			Func<T1, Task<TResult>> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[1] { typeof(T1) }, (object[] args) => handler2((T1)args[0]));
		}

		public static IDisposable On<T1, T2, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, Task<TResult>> handler)
		{
			Func<T1, T2, Task<TResult>> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[2]
			{
				typeof(T1),
				typeof(T2)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1]));
		}

		public static IDisposable On<T1, T2, T3, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, Task<TResult>> handler)
		{
			Func<T1, T2, T3, Task<TResult>> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[3]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2]));
		}

		public static IDisposable On<T1, T2, T3, T4, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, Task<TResult>> handler)
		{
			Func<T1, T2, T3, T4, Task<TResult>> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[4]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]));
		}

		public static IDisposable On<T1, T2, T3, T4, T5, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, Task<TResult>> handler)
		{
			Func<T1, T2, T3, T4, T5, Task<TResult>> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[5]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4]));
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, Task<TResult>> handler)
		{
			Func<T1, T2, T3, T4, T5, T6, Task<TResult>> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[6]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5]));
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6, T7, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> handler)
		{
			Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[7]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6]));
		}

		public static IDisposable On<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this HubConnection hubConnection, string methodName, Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> handler)
		{
			Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> handler2 = handler;
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			return hubConnection.On(methodName, new Type[8]
			{
				typeof(T1),
				typeof(T2),
				typeof(T3),
				typeof(T4),
				typeof(T5),
				typeof(T6),
				typeof(T7),
				typeof(T8)
			}, (object[] args) => handler2((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5], (T7)args[6], (T8)args[7]));
		}

		public static Task SendAsync(this HubConnection hubConnection, string methodName, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.SendCoreAsync(methodName, Array.Empty<object>(), cancellationToken);
		}

		public static Task SendAsync(this HubConnection hubConnection, string methodName, object? arg1, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.SendCoreAsync(methodName, new object[1] { arg1 }, cancellationToken);
		}

		public static Task SendAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.SendCoreAsync(methodName, new object[2] { arg1, arg2 }, cancellationToken);
		}

		public static Task SendAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.SendCoreAsync(methodName, new object[3] { arg1, arg2, arg3 }, cancellationToken);
		}

		public static Task SendAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.SendCoreAsync(methodName, new object[4] { arg1, arg2, arg3, arg4 }, cancellationToken);
		}

		public static Task SendAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.SendCoreAsync(methodName, new object[5] { arg1, arg2, arg3, arg4, arg5 }, cancellationToken);
		}

		public static Task SendAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.SendCoreAsync(methodName, new object[6] { arg1, arg2, arg3, arg4, arg5, arg6 }, cancellationToken);
		}

		public static Task SendAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.SendCoreAsync(methodName, new object[7] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 }, cancellationToken);
		}

		public static Task SendAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.SendCoreAsync(methodName, new object[8] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 }, cancellationToken);
		}

		public static Task SendAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.SendCoreAsync(methodName, new object[9] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 }, cancellationToken);
		}

		public static Task SendAsync(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.SendCoreAsync(methodName, new object[10] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 }, cancellationToken);
		}

		public static Task<ChannelReader<TResult>> StreamAsChannelAsync<TResult>(this HubConnection hubConnection, string methodName, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsChannelCoreAsync<TResult>(methodName, Array.Empty<object>(), cancellationToken);
		}

		public static Task<ChannelReader<TResult>> StreamAsChannelAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsChannelCoreAsync<TResult>(methodName, new object[1] { arg1 }, cancellationToken);
		}

		public static Task<ChannelReader<TResult>> StreamAsChannelAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsChannelCoreAsync<TResult>(methodName, new object[2] { arg1, arg2 }, cancellationToken);
		}

		public static Task<ChannelReader<TResult>> StreamAsChannelAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsChannelCoreAsync<TResult>(methodName, new object[3] { arg1, arg2, arg3 }, cancellationToken);
		}

		public static Task<ChannelReader<TResult>> StreamAsChannelAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsChannelCoreAsync<TResult>(methodName, new object[4] { arg1, arg2, arg3, arg4 }, cancellationToken);
		}

		public static Task<ChannelReader<TResult>> StreamAsChannelAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsChannelCoreAsync<TResult>(methodName, new object[5] { arg1, arg2, arg3, arg4, arg5 }, cancellationToken);
		}

		public static Task<ChannelReader<TResult>> StreamAsChannelAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsChannelCoreAsync<TResult>(methodName, new object[6] { arg1, arg2, arg3, arg4, arg5, arg6 }, cancellationToken);
		}

		public static Task<ChannelReader<TResult>> StreamAsChannelAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsChannelCoreAsync<TResult>(methodName, new object[7] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 }, cancellationToken);
		}

		public static Task<ChannelReader<TResult>> StreamAsChannelAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsChannelCoreAsync<TResult>(methodName, new object[8] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 }, cancellationToken);
		}

		public static Task<ChannelReader<TResult>> StreamAsChannelAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsChannelCoreAsync<TResult>(methodName, new object[9] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 }, cancellationToken);
		}

		public static Task<ChannelReader<TResult>> StreamAsChannelAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsChannelCoreAsync<TResult>(methodName, new object[10] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 }, cancellationToken);
		}

		public static async Task<ChannelReader<TResult>> StreamAsChannelCoreAsync<TResult>(this HubConnection hubConnection, string methodName, object?[] args, CancellationToken cancellationToken = default(CancellationToken))
		{
			_003C6bbd69be_002Dae3f_002D4258_002D9ae0_002Dbc12abae81ba_003EArgumentNullThrowHelper.ThrowIfNull(hubConnection, "hubConnection");
			ChannelReader<object> inputChannel = await hubConnection.StreamAsChannelCoreAsync(methodName, typeof(TResult), args, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			Channel<TResult> channel = Channel.CreateUnbounded<TResult>();
			RunChannel(inputChannel, channel);
			return channel.Reader;
		}

		private static async Task RunChannel<TResult>(ChannelReader<object> inputChannel, Channel<TResult> outputChannel)
		{
			_ = 1;
			try
			{
				while (await inputChannel.WaitToReadAsync().ConfigureAwait(continueOnCapturedContext: false))
				{
					object item;
					while (inputChannel.TryRead(out item))
					{
						while (!outputChannel.Writer.TryWrite((TResult)item))
						{
							if (!(await outputChannel.Writer.WaitToWriteAsync().ConfigureAwait(continueOnCapturedContext: false)))
							{
								return;
							}
						}
					}
				}
			}
			catch (Exception error)
			{
				outputChannel.Writer.TryComplete(error);
			}
			finally
			{
				outputChannel.Writer.TryComplete();
				_ = inputChannel.Completion.Exception;
			}
		}

		public static IAsyncEnumerable<TResult> StreamAsync<TResult>(this HubConnection hubConnection, string methodName, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsyncCore<TResult>(methodName, Array.Empty<object>(), cancellationToken);
		}

		public static IAsyncEnumerable<TResult> StreamAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsyncCore<TResult>(methodName, new object[1] { arg1 }, cancellationToken);
		}

		public static IAsyncEnumerable<TResult> StreamAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsyncCore<TResult>(methodName, new object[2] { arg1, arg2 }, cancellationToken);
		}

		public static IAsyncEnumerable<TResult> StreamAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsyncCore<TResult>(methodName, new object[3] { arg1, arg2, arg3 }, cancellationToken);
		}

		public static IAsyncEnumerable<TResult> StreamAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsyncCore<TResult>(methodName, new object[4] { arg1, arg2, arg3, arg4 }, cancellationToken);
		}

		public static IAsyncEnumerable<TResult> StreamAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsyncCore<TResult>(methodName, new object[5] { arg1, arg2, arg3, arg4, arg5 }, cancellationToken);
		}

		public static IAsyncEnumerable<TResult> StreamAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsyncCore<TResult>(methodName, new object[6] { arg1, arg2, arg3, arg4, arg5, arg6 }, cancellationToken);
		}

		public static IAsyncEnumerable<TResult> StreamAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsyncCore<TResult>(methodName, new object[7] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 }, cancellationToken);
		}

		public static IAsyncEnumerable<TResult> StreamAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsyncCore<TResult>(methodName, new object[8] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 }, cancellationToken);
		}

		public static IAsyncEnumerable<TResult> StreamAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsyncCore<TResult>(methodName, new object[9] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 }, cancellationToken);
		}

		public static IAsyncEnumerable<TResult> StreamAsync<TResult>(this HubConnection hubConnection, string methodName, object? arg1, object? arg2, object? arg3, object? arg4, object? arg5, object? arg6, object? arg7, object? arg8, object? arg9, object? arg10, CancellationToken cancellationToken = default(CancellationToken))
		{
			return hubConnection.StreamAsyncCore<TResult>(methodName, new object[10] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 }, cancellationToken);
		}
	}
}
