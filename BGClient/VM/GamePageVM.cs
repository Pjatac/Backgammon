using BGClient.Infra;
using BGClient.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;

namespace BGClient.VM
{
	public class GamePageVM : ViewModelBase, INavigable
	{
		private readonly IMessageService _messageService;
		private readonly IGameService _gameService;
		private readonly INavigationService _navigationService;
		private readonly IHubService _hubService;
		public bool FirstThrow = true;
		public int Priority { get; set; }
		public ObservableCollection<Cell> PlayGround { get; set; }
		public Board MyBoard { get; set; }
		public string Message { get; set; }
		public RelayCommand ThrowDicesCommand { get; private set; }
		public RelayCommand BackCommand { get; private set; }
		public RelayCommand<Cell> CellSelectedCommand { get; private set; }
		public bool From { get; set; }
		public int Start { get; set; }
		public int End { get; set; }
		private bool First = true;
		public GamePageVM(IMessageService messageService, INavigationService navigationService, IGameService gameService, IHubService hubService)
		{
			_hubService = hubService;
			_gameService = gameService;
			_messageService = messageService;
			_navigationService = navigationService;
			InitCommand();
		}
		private void InitCommand()
		{
			CellSelectedCommand = new RelayCommand<Cell>(async (cell) =>
			{
				if (From)
				{
					if ((cell.ChipColor == 1 && Priority == 1) || (cell.ChipColor == 2 && Priority == 2))
					{
						Start = Array.IndexOf(MyBoard.MyCells, cell);
						From = false;
						RaisePropertyChanged(() => Start);
					}
					else
					{
						await _messageService.Show("You cant check foreign cell");
					}
				}
				else
				{
					RaisePropertyChanged(() => End);
					if ((cell.ChipColor == 1 && Priority == 1) || (cell.ChipColor == 2 && Priority == 2) || (cell.ChipColor == 0))
					{
						End = Array.IndexOf(MyBoard.MyCells, cell);
						if (End > Start && Priority == 1)
						{
							int moveLength = End - Start;
							if (MyBoard.Moves.Any(m => m == moveLength))
							{
								MyBoard.Moves.Remove(moveLength);
								MyBoard.MyCells[Start].NumOfChip -= 1;
								if (MyBoard.MyCells[Start].NumOfChip == 0)
								{
									MyBoard.MyCells[Start].ChipColor = 0;
								}
								MyBoard.MyCells[End].NumOfChip += 1;
								if (MyBoard.MyCells[End].ChipColor == 0)
								{
									MyBoard.MyCells[End].ChipColor = 1;
								}
								await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
								async () =>
								{
									PlayGround = new ObservableCollection<Cell>(MyBoard.MyCells);
									DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged(() => PlayGround); });
								});
								if (MyBoard.Moves.Count == 0)
								{
									MyBoard.Dice1.Active = false;
									MyBoard.Dice2.Active = false;
									Start = 0;
									End = 0;
									DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged(() => Start); RaisePropertyChanged(() => End); });
									await _messageService.ShowContent($"Wating for opponent move...");
									await _hubService.Move(MyBoard.MyCells, MyBoard.User2);
								}
								From = true;
							}
							else
							{
								await _messageService.Show("You cant do such move");
							}
						}
						else if(End < Start && Priority == 2)
						{
							int moveLength = Start - End;
							if (MyBoard.Moves.Any(m => m == moveLength))
							{
								MyBoard.Moves.Remove(moveLength);
								MyBoard.MyCells[Start].NumOfChip -= 1;
								if (MyBoard.MyCells[Start].NumOfChip == 0)
								{
									MyBoard.MyCells[Start].ChipColor = 0;
								}
								MyBoard.MyCells[End].NumOfChip += 1;
								if (MyBoard.MyCells[End].ChipColor == 0)
								{
									MyBoard.MyCells[End].ChipColor = 2;
								}
								await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
								async () =>
								{
									PlayGround = new ObservableCollection<Cell>(MyBoard.MyCells);
									DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged(() => PlayGround); });
								});
								if (MyBoard.Moves.Count == 0)
								{
									MyBoard.Dice1.Active = false;
									MyBoard.Dice2.Active = false;
									Start = 0;
									End = 0;
									DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged(() => Start); RaisePropertyChanged(() => End); });
									await _messageService.ShowContent($"Wating for opponent move...");
									await _hubService.Move(MyBoard.MyCells, MyBoard.User2);
								}
								From = true;
							}
							else
							{
								await _messageService.Show("You cant do such move");
							}
						}
					}
					else
					{
						await _messageService.Show("You cant check foreign cell");
					}
				}
			});
			ThrowDicesCommand = new RelayCommand(async () =>
			{
				_gameService.ThrowDice(MyBoard.Dice1);
				_gameService.ThrowDice(MyBoard.Dice2);
				await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
				async () =>
				{
					RaisePropertyChanged(() => MyBoard.Dice1);
					RaisePropertyChanged(() => MyBoard.Dice2);
				});
				if (FirstThrow)
				{
					await _hubService.FirstThrow(MyBoard.Dice1.Number + MyBoard.Dice2.Number, MyBoard.User2);
					FirstThrow = false;
					MyBoard.Dice1.Active = false;
					MyBoard.Dice2.Active = false;
					await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
					async () =>
					{
						RaisePropertyChanged(() => MyBoard.Dice1);
						RaisePropertyChanged(() => MyBoard.Dice2);
					});
					MyBoard.Dice1.Active = false;
					RaisePropertyChanged(() => MyBoard.Dice1);
				}
				else
				{
					MyBoard.Moves.Add(MyBoard.Dice1.Number);
					MyBoard.Moves.Add(MyBoard.Dice2.Number);
				}
				From = true;
			});
			BackCommand = new RelayCommand(async () =>
			{
				_navigationService.NavigateTo("HomePage");
			});
		}
		public async Task GetMove(Cell[] MyCells)
		{
			MyBoard.MyCells = MyCells;
			await _messageService.HideContent();
			PlayGround = new ObservableCollection<Cell>(MyCells);
			DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged(() => PlayGround); });
		}
		public async Task GetPriority(int priority)
		{		
			Priority = priority;
			DispatcherHelper.CheckBeginInvokeOnUI(() => { RaisePropertyChanged(() => Priority); }); 
			if (Priority == 2)
			{
				await _messageService.HideContent();
				await _messageService.ShowContent($"Wating for opponent move...");
			}
			else
			{
				await _messageService.HideContent();
			}
		}
		public void OnNavigatedTo(NavigationEventArgs e)
		{
			MyBoard = e.Parameter as Board;
			MyBoard.Dice1 = new Dice();
			MyBoard.Dice2 = new Dice();
			MyBoard.Moves = new List<int>();
			if (First)
			{
				_hubService.RegisterAction("GetMove", async (object cells) => await GetMove(cells as Cell[]));
				_hubService.RegisterAction("GetPriority", async (object priority) => await GetPriority(Convert.ToInt32(priority)));
				PlayGround = new ObservableCollection<Cell>(MyBoard.MyCells);
			}
			RaisePropertyChanged(() => MyBoard);
			_messageService.Show("For start you need throw dices!");
			First = false;
		}

		public void OnNavigatedFrom(NavigationEventArgs e)
		{
		}

		public void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{

		}

		public bool AllowGoBack()
		{
			throw new NotImplementedException();
		}
	}
}

