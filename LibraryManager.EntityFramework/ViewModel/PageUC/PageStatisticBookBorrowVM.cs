﻿using LibraryManager.EntityFramework.Model;
using LibraryManager.Utils;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManager.EntityFramework.ViewModel
{
   public class PageStatisticBookBorrowVM : BaseViewModel
   {
      public ObservableCollection<BorrowDTO> ListBookBorrow { get => listBookBorrow; set { listBookBorrow = value; OnPropertyChanged(); } }
      public BookDTO BookStatistic { get => bookStatistic; set { bookStatistic = value; OnPropertyChanged(); } }

      public ICommand BackCommand { get; set; }

      public PageStatisticBookBorrowVM(BookDTO bookStatistic)
      {
         BookStatistic = bookStatistic;
         ListBookBorrow = BorrowDAL.Instance.GetListByBookId(bookStatistic.Id);

         BackCommand = new RelayCommand<UserControl>((p) => { return p != null; }, (p) =>
         {
            try
            {
               var w = p.GetRootParent() as Window;
               var gridMain = w.FindName("gridMain") as Grid;
               gridMain.Children.Remove(p);
            }
            catch (System.Exception) { }
         });
      }

      private ObservableCollection<BorrowDTO> listBookBorrow;
      private BookDTO bookStatistic;
   }
}