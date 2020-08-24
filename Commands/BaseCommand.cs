using System;
using System.Threading.Tasks;
using CommandLine;

namespace pvcms.Commands
{
    public abstract class BaseCommand
    {

        public const int TabSize = 2;

        // Interface by which we execute our commands.
        protected abstract void OnExecute();

        public void Execute()
        {
            OnExecute();
        }

        public string Tab(int size)
        {
            return new string(' ', size);
        }


    }
}
