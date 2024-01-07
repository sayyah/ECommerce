namespace ECommerce.Domain.Entities.HolooEntity;

public class HolooSndList(int sanad_Code, string col_Code, string moien_Code, string tafzili_Code, double? bed,
    double? bes, string comment_Line)
{
    public int Sanad_Code { get; set; } = sanad_Code;
    public int Index { get; set; }
    public string Col_Code { get; set; } = col_Code;
    public string Moien_Code { get; set; } = moien_Code;
    public string Tafzili_Code { get; set; } = tafzili_Code;
    public double? Bed { get; set; } = bed;
    public double? Bes { get; set; } = bes;
    public bool? Show_Daftar { get; set; } = true;
    public bool? Joze { get; set; } = false;
    public byte? Actions { get; set; } = 0;
    public string Comment_Line { get; set; } = comment_Line;
    public int OldSCode { get; set; } = 0;
    public double Bed_Arz { get; set; } = 0;
    public double Bes_Arz { get; set; } = 0;
    public int ArzId { get; set; } = 1;
    public double Money_Price { get; set; } = 1;
    public double Money_change { get; set; } = 1;
}
