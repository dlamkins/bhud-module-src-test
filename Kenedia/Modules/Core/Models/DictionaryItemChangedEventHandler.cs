namespace Kenedia.Modules.Core.Models
{
	public delegate void DictionaryItemChangedEventHandler<TKey, TValue>(object sender, DictionaryItemChangedEventArgs<TKey, TValue> e);
}
