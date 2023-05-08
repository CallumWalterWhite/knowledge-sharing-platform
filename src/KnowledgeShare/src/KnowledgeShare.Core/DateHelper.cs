namespace KnowledgeShare.Core;

public static class DateHelper
{
    public static string GetCommentDate(DateTime localDate)
    {
        DateTime today = DateTime.Now;
        double totalDays = Convert.ToInt32((today - localDate).TotalDays);
        if (totalDays > 0)
        {
            return $"{totalDays} days ago";
        }

        double hours = Convert.ToInt32((today - localDate).TotalHours);
        if (hours > 0)
        {
            return $"{hours} hours ago";
        }
        
        return $"{Convert.ToInt32((today - localDate).TotalMinutes)} minutes ago"; 
    }
}