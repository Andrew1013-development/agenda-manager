using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* exit code
* 0 = normal exit
* 1 = database connection cannot be established
* 2 = cannot retrieve data from collection in database
* 3 = invalid role specified
* 4 = invalid input (empty field or invalid string to parse)
* 5 = wrong password
* 6 = cannot upload data to database
* 7 = secret menu accessed
* 8 = cannot fetch updates
* 9 = cannot download updates
* 10 = cannot install updates
* 11 = unknown exit code
*/

namespace AgendaLibrary.Definitions
{
    public enum exitCode
    {
        SuccessfulExecution = 0,
        DatabaseConnectionFailure = 1,
        CollectionRetrievalFailure = 2,
        InvalidRole = 3,
        InvalidInput = 4,
        WrongPassword = 5,
        DatabaseUploadFailure = 6,
        SecretMenuAccessed = 7,
        FetchUpdateFailure = 8,
        DownloadUpdateFailure = 9,
        InstallUpdateFailure = 10,
        UnknownExitCode = 11,
    }
}
