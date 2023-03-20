namespace Nico
{
    public interface IPoolObject
    {
        void OnGet();
        void Return();
    }
}