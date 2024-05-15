using PearsonXApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PearsonXApp.UseCases.CommandExecutor
{
    public interface ICommandExecutor
    {
        void ExecuteCommand(string[] commandParts);
    }
}
