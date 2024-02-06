using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using lab_3.ViewModels;

namespace lab_3.Views;

public partial class ChampionView : UserControl
{
    public ChampionView()
    {
        InitializeComponent();
        DataContext = new ChampionViewModel();
    }
}