namespace ECommerce.Domain.Entities.HolooEntity;

public class HolooSanad(string comment) : BaseHolooEntity
{
    public int Sanad_Code { get; set; }
    public int? Sanad_Code_C { get; set; }
    public int? Sanad_Code_C2 { get; set; } = 0;
    public DateTime? Sanad_Date { get; set; } = DateTime.Now.Date;
    public DateTime? Sanad_Time { get; set; } = new DateTime(1900, 1, 1, DateTime.Now.ToLocalTime().Hour, DateTime.Now.ToLocalTime().Minute,
        DateTime.Now.ToLocalTime().Second);

    public string Comment { get; set; } = comment;
    public short? Sanad_Type { get; set; } = 1;
    public string DateUser { get; set; } = DateTime.Now.ToString("yyyy/MM/dd");
    public string TimeUser { get; set; } = DateTime.Now.ToString("HH:mm");
    public int? UserCodeInc { get; set; } = 1;
    public DateTime? Endeditdate { get; set; } = DateTime.Now;
    public short sanad_state { get; set; } = 0;
    public bool Transfer_Recive_Snd { get; set; } = false;
    public bool Transfer_Send_Snd { get; set; } = false;
    public int FCode_ChangeGold { get; set; } = 0;
}
