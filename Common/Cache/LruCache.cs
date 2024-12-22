﻿namespace Common.Cache;

public class LruCache<TK, TV> where TK : notnull
{
	private readonly int _capacity;
	private readonly Dictionary<TK, LinkedListNode<(TK Key, TV Value)>> _cache;
	private readonly LinkedList<(TK Key, TV Value)> _order;

	public LruCache(int capacity)
	{
		if (capacity <= 0)
		{
			throw new ArgumentException("Capacity must be greater than zero.");
		}

		_capacity = capacity;
		_cache = new Dictionary<TK, LinkedListNode<(TK, TV)>>(capacity);
		_order = new LinkedList<(TK, TV)>();
	}

	public TV Get(TK key)
	{
		if (!_cache.TryGetValue(key, out var node))
		{
			throw new KeyNotFoundException("Key not found in cache.");
		}

		_order.Remove(node);
		_order.AddFirst(node);
		return node.Value.Value;
	}

	public void Put(TK key, TV value)
	{
		if (_cache.TryGetValue(key, out var node))
		{
			node.Value = (key, value);
			_order.Remove(node);
			_order.AddFirst(node);
		}
		else
		{
			if (_cache.Count >= _capacity)
			{
				if (_order.Last != null)
				{
					var lru = _order.Last.Value;
					_order.RemoveLast();
					_cache.Remove(lru.Key);
				}
			}

			var newNode = new LinkedListNode<(TK, TV)>((key, value));
			_order.AddFirst(newNode);
			_cache[key] = newNode;
		}
	}

	public void PrintCache()
	{
		foreach (var item in _order)
		{
			Console.Write($"[{item.Key}:{item.Value}] ");
		}

		Console.WriteLine();
	}
}