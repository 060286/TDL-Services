using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDL.Domain.Helper
{
    public static class MigrationHelper
    {
        public static bool IsMigrationOperationExcuting()
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
            string[] orderedMigrationArguments = { "migrations", "add" };

            for(var i = 0; i <= commandLineArguments.Length - orderedMigrationArguments.Length; i++)
            {
                if (commandLineArguments.Skip(i).Take(orderedMigrationArguments.Length).SequenceEqual(orderedMigrationArguments))
                    return true;
            }

            return false;
        }
    }
}
