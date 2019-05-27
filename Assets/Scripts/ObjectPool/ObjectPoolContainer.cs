namespace ObjectPool
{
    public class ObjectPoolContainer<T>
    {
        private T _item;

        public bool Used { get; private set; }

        public void Consume()
        {
            Used = true;
        }

        public T Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
            }
        }

        public void Release()
        {
            Used = false;
        }
    }
}