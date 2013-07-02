namespace Autofac
{
    /// <summary>
    ///     Used to force the container to activate a component once when the configuration has completed.
    ///     See http://code.google.com/p/autofac/wiki/Startable for details
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class StartableBootstrap<T> : IStartable
    {
        private readonly IComponentContext _context;

        public StartableBootstrap(IComponentContext context)
        {
            _context = context;
        }

        public void Start()
        {
            _context.Resolve<T>();
        }
    }
}