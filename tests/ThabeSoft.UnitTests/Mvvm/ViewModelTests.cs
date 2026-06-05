namespace ThabeSoft.Mvvm;


[TestClass]
public class ViewModelTests : ViewModel
{
    public int Number
    {
        get => field;
        set => SetProperty(field, value)
            .OnChanged(x => field = x.NewValue)
            .Apply();
    }


    [TestMethod]
    public void Fuck()
    {
        SetProperty<int>()
    }
}
