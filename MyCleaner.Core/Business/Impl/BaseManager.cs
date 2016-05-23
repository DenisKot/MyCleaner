namespace MyCleaner.Core.Business.Impl
{
    public abstract class BaseManager : IManager
    {
        protected bool running = true;

        public abstract void Work(object param);

        public void Canel()
        {
            running = false;
        }
    }
}
