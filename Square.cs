public class Square {
    public int? FinalValue { get; set; }
    public List<int> PossibleValues { get; set; }
    public bool? IsValid {get; set;}

    public Square() : this(null)
    {                    
    }

    public Square(int? finalValue)
    {           
        FinalValue = finalValue;        

        PossibleValues = new List<int>();          

        if (FinalValue == null) {
            for(int i = 1; i <= 9; i++) {
                PossibleValues.Add(i);
            }

            IsValid = null;
        } 
        else {
            IsValid = false;
        }
    }
}

