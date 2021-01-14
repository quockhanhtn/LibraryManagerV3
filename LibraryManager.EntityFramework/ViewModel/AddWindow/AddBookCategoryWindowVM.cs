using LibraryManager.EntityFramework.Model;
using LibraryManager.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManager.EntityFramework.ViewModel
{
   public class AddBookCategoryWindowVM : BaseViewModel, IAddNewObject<BookCategoryDTO>
   {
      public BookCategoryDTO Result { get; set; }

      public ICommand OKCommand { get; set; }
      public ICommand CancelCommand { get; set; }

      public AddBookCategoryWindowVM()
      {
         OKCommand = new RelayCommand<Window>((p) => { return !(p == null); }, (p) =>
         {
            var txtName = p.FindName("txtName") as TextBox;
            var txtLimitDays = p.FindName("txtLimitDays") as TextBox;
            var tblNameWarning = p.FindName("tblNameWarning") as TextBlock;
            var tblLimitDaysWarning = p.FindName("tblLimitDaysWarning") as TextBlock;

            if (txtName.Text == "")
            {
               tblNameWarning.Visibility = Visibility.Visible;
               txtName.Focus();
               return;
            }
            else { tblNameWarning.Visibility = Visibility.Hidden; }

            if (txtLimitDays.Text.ToInt() == 0)
            {
               tblLimitDaysWarning.Visibility = Visibility.Visible;
               txtLimitDays.Focus();
               return;
            }
            else { tblLimitDaysWarning.Visibility = Visibility.Hidden; }

            BookCategoryDTO newBookCategory = new BookCategoryDTO()
            {
               Name = txtName.Text,
               LimitDays = txtLimitDays.Text.ToInt(),
               Status = true
            };

            BookCategoryDAL.Instance.Add(newBookCategory);
            Result = newBookCategory;
            p.Close();
         });

         CancelCommand = new RelayCommand<Window>((p) => { return !(p == null); }, (p) => { p.Close(); });
      }
   }
}