using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDL.Services.Dto.TodoDto
{
    public class ViewArchivedTaskReportResponseDto
    {
        public ViewArchivedTaskCountItem ArchivedCounter { get; set; }

        public IList<ViewArchivedTaskItem> ArchivedTaskList { get; set; }
    }

    public class ViewArchivedTaskItem
    {
        public DateTime ArchivedDate { get; set; }

        public IList<ViewArchivedTask> ArchivedTasks { get; set; }
    }

    public class ViewArchivedTask
    {
        public bool IsArchived { get; set; }

        public string Title { get; set; }

        public string CategoryTitle { get; set; }
    }

    public class ViewArchivedTaskCountItem
    {
        public int TotalItemCount { get; set; }

        public int ThisWeekCount { get; set; }

        public int TodayCount { get; set; }
    }
}

