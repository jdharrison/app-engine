namespace Core
{
    public class DataInstance
    {
        private static double _totalInstances;

        public string id
        { get; private set; }

        public DataInstance()
        {
            _totalInstances++;
            id = _totalInstances.ToString();
        }
    }
}