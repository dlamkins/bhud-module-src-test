namespace System.Threading.Channels
{
	internal enum BoundedChannelFullMode
	{
		Wait,
		DropNewest,
		DropOldest,
		DropWrite
	}
}
