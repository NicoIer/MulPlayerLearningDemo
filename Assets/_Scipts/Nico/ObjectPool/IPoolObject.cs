namespace Kitchen
{
    public interface IPoolObject
    {
        void OnGet();
        void Return();
    }
}