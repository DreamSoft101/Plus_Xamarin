using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Dex.Com.Expresso.Loyalty.Droid.Widgets.AsymmetricGridView
{
    public class ObjectPool<T> : IParcelable where T : struct
    {
        Stack<T> stack = new Stack<T>();
        PoolObjectFactory<T> factory;
        PoolStats stats;

        public ObjectPool(Parcel input)
        {
        }

        public ObjectPool()
        {
            stats = new PoolStats();
        }

        public ObjectPool(PoolObjectFactory<T> factory)
        {
            this.factory = factory;
        }

        public T get()
        {
            if (stack.Count != 0)
            {
                stats.hits++;
                stats.size--;
                return stack.Pop();
            }

            stats.misses++;

            T obj = new T(); ;
            if (factory != null)
            {
                obj = factory.createObject();
            }

            if (obj != null)
            {
                stats.created++;
            }

            return obj;
        }

        public IntPtr Handle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int DescribeContents()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags)
        {
            throw new NotImplementedException();
        }


        public class PoolStats
        {
            public int size = 0;
            public int hits = 0;
            public int misses = 0;
            public int created = 0;

            public string getStats(String name)
            {
                return String.Format("%s: size %d, hits %d, misses %d, created %d", name, size, hits,misses, created);
            }
        }

    }
}