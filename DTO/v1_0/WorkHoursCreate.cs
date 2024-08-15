using System;

namespace DTO.v1_0;

public class WorkHoursCreate
{
    public Guid UserWorkId { get; set; } 
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool LunchBreak { get; set; }
}