using Gtk;

namespace IFACARS.Types.Gtk
{
    public abstract class StackScreen
    {
        protected Stack _parentStack;
        private string _xmlName;

        public StackScreen(Stack parentStack, string xmlName)
        {
            _parentStack = parentStack;
            _xmlName = xmlName;
        }

        public abstract void Init();

        protected void SwitchToThisScreen()
        {
            _parentStack.VisibleChildName = _xmlName;
        }
    }
}