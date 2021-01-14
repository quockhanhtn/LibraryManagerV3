using LibraryManager.EntityFramework.Model;
using LibraryManager.EntityFramework.View;
using LibraryManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManager.EntityFramework.ViewModel
{
   public class AddBookWindowVM : BaseViewModel, IAddNewObject<BookDTO>
   {
      public ObservableCollection<BookCategoryDTO> ListBookCategory { get => listBookCategory; set { listBookCategory = value; OnPropertyChanged(); } }
      public ObservableCollection<PublisherDTO> ListPublisher { get => listPublisher; set { listPublisher = value; OnPropertyChanged(); } }
      public ObservableCollection<Author> ListAuthor { get => listAuthor; set { listAuthor = value; OnPropertyChanged(); } }
      public ObservableCollection<Author> ListBookAuthor { get => listBookAuthor; set { listBookAuthor = value; OnPropertyChanged(); } }
      public BookCategoryDTO BookCategorySelected { get => bookCategorySelected; set { bookCategorySelected = value; OnPropertyChanged(); } }
      public PublisherDTO PublisherSelected { get => publisherSelected; set { publisherSelected = value; OnPropertyChanged(); } }
      public Author AuthorSelected { get => authorSelected; set { authorSelected = value; OnPropertyChanged(); } }
      public Author BookAuthorSelected { get => bookAuthorSelected; set { bookAuthorSelected = value; OnPropertyChanged(); } }
      public ICommand LoadedCommand { get; set; }
      public ICommand AddBookCategoryCommand { get; set; }
      public ICommand AddPublisherCommand { get; set; }
      public ICommand AddAuthorCommand { get; set; }
      public ICommand AddAuthorToListCommand { get; set; }
      public ICommand DeleteAuthorFromListCommand { get; set; }
      public ICommand OKCommand { get; set; }
      public ICommand RetypeCommand { get; set; }
      public ICommand CancelCommand { get; set; }

      public BookDTO Result { get; set; }

      public AddBookWindowVM()
      {
         ListBookCategory = BookCategoryDAL.Instance.GetList(EStatusFillter.Active);
         ListPublisher = PublisherDAL.Instance.GetList(EStatusFillter.Active);
         ListAuthor = AuthorDAL.Instance.GetRawList();
         ListBookAuthor = new ObservableCollection<Author>();

         LoadedCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
          {
             var txtTitle = p.FindName("txtTitle") as TextBox;
             txtTitle.Focus();
          });

         AddBookCategoryCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
           {
              var addDataContext = new AddBookCategoryWindowVM();
              var addBookCategoryWindow = new AddBookCategoryWindow() { DataContext = addDataContext };
              addBookCategoryWindow.ShowDialog();

              if (addDataContext.Result != null)
              {
                 ListBookCategory = BookCategoryDAL.Instance.GetList(EStatusFillter.Active);
                 var cmbBookCategory = p.FindName("cmbBookCategory") as ComboBox;
                 cmbBookCategory.SelectedIndex = ListBookCategory.Count - 1;
              }
           });

         AddPublisherCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
          {
             var addDataContext = new AddPublisherWindowVM();
             var addPublisherWindow = new AddPublisherWindow() { DataContext = addDataContext };
             addPublisherWindow.ShowDialog();

             if (addDataContext.Result != null)
             {
                ListPublisher = PublisherDAL.Instance.GetList(EStatusFillter.Active);
                var cmbPublisher = p.FindName("cmbPublisher") as ComboBox;
                cmbPublisher.SelectedIndex = ListPublisher.Count - 1;
             }
          });

         AddAuthorCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
          {
             var addDataContext = new AddAuthorWindowVM();
             var addAuthorWindow = new AddAuthorWindow() { DataContext = addDataContext };
             addAuthorWindow.ShowDialog();

             if (addDataContext.Result != null)
             {
                ListAuthor = AuthorDAL.Instance.GetRawList();

                foreach (var item in ListBookAuthor) { ListAuthor.Remove(item); }

                var cmbAuthor = p.FindName("cmbAuthor") as ComboBox;
                cmbAuthor.SelectedIndex = ListAuthor.Count - 1;
             }
          });

         AddAuthorToListCommand = new RelayCommand<object>((p) => { return AuthorSelected != null; }, (p) =>
          {
             ListBookAuthor.Add(AuthorSelected);
             ListAuthor.Remove(AuthorSelected);
          });

         DeleteAuthorFromListCommand = new RelayCommand<object>((p) => { return BookAuthorSelected != null; }, (p) =>
          {
             ListAuthor.Add(BookAuthorSelected);
             ListBookAuthor.Remove(BookAuthorSelected);
          });

         OKCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
          {
             var txtTitle = p.FindName("txtTitle") as TextBox;
             var txtYearPublish = p.FindName("txtYearPublish") as TextBox;
             var txtPageNumber = p.FindName("txtPageNumber") as TextBox;
             var txtSize = p.FindName("txtSize") as TextBox;
             var txtPrice = p.FindName("txtPrice") as TextBox;
             var txtNumber = p.FindName("txtNumber") as TextBox;

             var tblTitleWarning = p.FindName("tblTitleWarning") as TextBlock;
             var tblBookCategoryWarning = p.FindName("tblBookCategoryWarning") as TextBlock;
             var tblPublisherWarning = p.FindName("tblPublisherWarning") as TextBlock;
             var tblYearPublishWarning = p.FindName("tblYearPublishWarning") as TextBlock;
             var tblAuthorWaning = p.FindName("tblAuthorWaning") as TextBlock;
             var tblPageNumberWarning = p.FindName("tblPageNumberWarning") as TextBlock;
             var tblPriceWarning = p.FindName("tblPriceWarning") as TextBlock;
             var tblNumberWarning = p.FindName("tblNumberWarning") as TextBlock;

             if (txtTitle.Text == "")
             {
                tblTitleWarning.Visibility = Visibility.Visible;
                txtTitle.Focus();
                return;
             }
             else { tblTitleWarning.Visibility = Visibility.Hidden; }

             if (BookCategorySelected == null)
             {
                tblBookCategoryWarning.Visibility = Visibility.Visible;
                return;
             }
             else { tblBookCategoryWarning.Visibility = Visibility.Hidden; }

             if (PublisherSelected == null)
             {
                tblPublisherWarning.Visibility = Visibility.Visible;
                return;
             }
             else { tblPublisherWarning.Visibility = Visibility.Hidden; }

             if (txtYearPublish.Text.ToInt() < 1900 || txtYearPublish.Text.ToInt() > DateTime.Now.Year)
             {
                tblYearPublishWarning.Visibility = Visibility.Visible;
                txtYearPublish.Focus();
                return;
             }
             else { tblYearPublishWarning.Visibility = Visibility.Hidden; }

             if (ListBookAuthor.Count == 0)
             {
                tblAuthorWaning.Visibility = Visibility.Visible;
                return;
             }
             else { tblAuthorWaning.Visibility = Visibility.Hidden; }

             if (txtPageNumber.Text.ToInt() == 0)
             {
                tblPageNumberWarning.Visibility = Visibility.Visible;
                txtPageNumber.Focus();
                return;
             }
             else { tblPageNumberWarning.Visibility = Visibility.Hidden; }

             if (txtPrice.Text.ToDecimal() == 0)
             {
                tblPriceWarning.Visibility = Visibility.Visible;
                txtPrice.Focus();
                return;
             }
             else { tblPriceWarning.Visibility = Visibility.Hidden; }

             if (txtNumber.Text.ToInt() == 0)
             {
                tblNumberWarning.Visibility = Visibility.Visible;
                txtNumber.Focus();
                return;
             }
             else { tblNumberWarning.Visibility = Visibility.Hidden; }

             var listAuthor = new List<Author>();
             foreach (var item in ListBookAuthor)
             {
                listAuthor.Add(new Author() { Id = item.Id, NickName = item.NickName });
             }

             BookDTO newBook = new BookDTO()
             {
                Title = txtTitle.Text.CapitalizeEachWord(),
                BookCategoryId = BookCategorySelected.Id,
                PublisherId = PublisherSelected.Id,
                YearPublish = txtYearPublish.Text.ToInt(),
                PageNumber = txtPageNumber.Text.ToInt(),
                Size = txtSize.Text,
                Price = txtPrice.Text.ToDecimal(),
                Authors = listAuthor
             };

             BookDAL.Instance.Add(newBook, txtNumber.Text.ToInt());
             Result = newBook;
             p.Close();
          });

         RetypeCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
         {
         });

         CancelCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) => { p.Close(); });
      }

      /// <summary>
      /// Update book
      /// </summary>
      /// <param name="bookUpdate"></param>
      public AddBookWindowVM(BookDTO bookUpdate)
      {
         ListBookCategory = BookCategoryDAL.Instance.GetList(EStatusFillter.Active);
         ListPublisher = PublisherDAL.Instance.GetList(EStatusFillter.Active);
         ListAuthor = AuthorDAL.Instance.GetRawList();
         ListBookAuthor = new ObservableCollection<Author>();
         foreach (var item in bookUpdate.Authors) { ListBookAuthor.Add(item); }

         BookCategorySelected = new BookCategoryDTO(bookUpdate.BookCategory);
         PublisherSelected = new PublisherDTO(bookUpdate.Publisher);

         LoadedCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
         {
            var txtTitle = p.FindName("txtTitle") as TextBox;
            var txtYearPublish = p.FindName("txtYearPublish") as TextBox;
            var txtPageNumber = p.FindName("txtPageNumber") as TextBox;
            var txtSize = p.FindName("txtSize") as TextBox;
            var txtPrice = p.FindName("txtPrice") as TextBox;
            var txtNumber = p.FindName("txtNumber") as TextBox;
            var btnOK = p.FindName("btnOK") as Button;

            txtTitle.Text = bookUpdate.Title;
            txtYearPublish.Text = bookUpdate.YearPublish.ToString();
            txtPageNumber.Text = bookUpdate.PageNumber.ToString();
            txtSize.Text = bookUpdate.Size;
            txtPrice.Text = bookUpdate.Price.ToString();
            txtNumber.Text = bookUpdate.BookItem.Number.ToString();
            btnOK.Content = "CẬP NHẬT";
         });

         AddBookCategoryCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
         {
            var addDataContext = new AddBookCategoryWindowVM();
            var addBookCategoryWindow = new AddBookCategoryWindow() { DataContext = addDataContext };
            addBookCategoryWindow.ShowDialog();

            if (addDataContext.Result != null)
            {
               ListBookCategory = BookCategoryDAL.Instance.GetList(EStatusFillter.Active);
               var cmbBookCategory = p.FindName("cmbBookCategory") as ComboBox;
               cmbBookCategory.SelectedIndex = ListBookCategory.Count - 1;
            }
         });

         AddPublisherCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
         {
            var addDataContext = new AddPublisherWindowVM();
            var addPublisherWindow = new AddPublisherWindow() { DataContext = addDataContext };
            addPublisherWindow.ShowDialog();

            if (addDataContext.Result != null)
            {
               ListPublisher = PublisherDAL.Instance.GetList(EStatusFillter.Active);
               var cmbPublisher = p.FindName("cmbPublisher") as ComboBox;
               cmbPublisher.SelectedIndex = ListPublisher.Count - 1;
            }
         });

         AddAuthorCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
         {
            var addDataContext = new AddAuthorWindowVM();
            var addAuthorWindow = new AddAuthorWindow() { DataContext = addDataContext };
            addAuthorWindow.ShowDialog();

            if (addDataContext.Result != null)
            {
               ListAuthor = AuthorDAL.Instance.GetRawList();

               foreach (var item in ListBookAuthor) { ListAuthor.Remove(item); }

               var cmbAuthor = p.FindName("cmbAuthor") as ComboBox;
               cmbAuthor.SelectedIndex = ListAuthor.Count - 1;
            }
         });

         AddAuthorToListCommand = new RelayCommand<object>((p) => { return AuthorSelected != null; }, (p) =>
         {
            ListBookAuthor.Add(AuthorSelected);
            ListAuthor.Remove(AuthorSelected);
         });

         DeleteAuthorFromListCommand = new RelayCommand<object>((p) => { return BookAuthorSelected != null; }, (p) =>
         {
            ListAuthor.Add(BookAuthorSelected);
            ListBookAuthor.Remove(BookAuthorSelected);
         });

         OKCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
         {
            var txtTitle = p.FindName("txtTitle") as TextBox;
            var txtYearPublish = p.FindName("txtYearPublish") as TextBox;
            var txtPageNumber = p.FindName("txtPageNumber") as TextBox;
            var txtSize = p.FindName("txtSize") as TextBox;
            var txtPrice = p.FindName("txtPrice") as TextBox;
            var txtNumber = p.FindName("txtNumber") as TextBox;

            var tblTitleWarning = p.FindName("tblTitleWarning") as TextBlock;
            var tblBookCategoryWarning = p.FindName("tblBookCategoryWarning") as TextBlock;
            var tblPublisherWarning = p.FindName("tblPublisherWarning") as TextBlock;
            var tblYearPublishWarning = p.FindName("tblYearPublishWarning") as TextBlock;
            var tblAuthorWaning = p.FindName("tblAuthorWaning") as TextBlock;
            var tblPageNumberWarning = p.FindName("tblPageNumberWarning") as TextBlock;
            var tblPriceWarning = p.FindName("tblPriceWarning") as TextBlock;
            var tblNumberWarning = p.FindName("tblNumberWarning") as TextBlock;

            if (txtTitle.Text == "")
            {
               tblTitleWarning.Visibility = Visibility.Visible;
               txtTitle.Focus();
               return;
            }
            else { tblTitleWarning.Visibility = Visibility.Hidden; }

            if (BookCategorySelected == null)
            {
               tblBookCategoryWarning.Visibility = Visibility.Visible;
               return;
            }
            else { tblBookCategoryWarning.Visibility = Visibility.Hidden; }

            if (PublisherSelected == null)
            {
               tblPublisherWarning.Visibility = Visibility.Visible;
               return;
            }
            else { tblPublisherWarning.Visibility = Visibility.Hidden; }

            if (txtYearPublish.Text.ToInt() < 1900 || txtYearPublish.Text.ToInt() > DateTime.Now.Year)
            {
               tblYearPublishWarning.Visibility = Visibility.Visible;
               txtYearPublish.Focus();
               return;
            }
            else { tblYearPublishWarning.Visibility = Visibility.Hidden; }

            if (ListBookAuthor.Count == 0)
            {
               tblAuthorWaning.Visibility = Visibility.Visible;
               return;
            }
            else { tblAuthorWaning.Visibility = Visibility.Hidden; }

            if (txtPageNumber.Text.ToInt() == 0)
            {
               tblPageNumberWarning.Visibility = Visibility.Visible;
               txtPageNumber.Focus();
               return;
            }
            else { tblPageNumberWarning.Visibility = Visibility.Hidden; }

            if (txtPrice.Text.ToDecimal() == 0)
            {
               tblPriceWarning.Visibility = Visibility.Visible;
               txtPrice.Focus();
               return;
            }
            else { tblPriceWarning.Visibility = Visibility.Hidden; }

            if (txtNumber.Text.ToInt() == 0)
            {
               tblNumberWarning.Visibility = Visibility.Visible;
               txtNumber.Focus();
               return;
            }
            else { tblNumberWarning.Visibility = Visibility.Hidden; }

            var listAuthor = new List<Author>();
            foreach (var item in ListBookAuthor)
            {
               listAuthor.Add(new Author() { Id = item.Id, NickName = item.NickName });
            }

            bookUpdate.Title = txtTitle.Text.CapitalizeEachWord();
            bookUpdate.BookCategoryId = BookCategorySelected.Id;
            bookUpdate.PublisherId = PublisherSelected.Id;
            bookUpdate.YearPublish = txtYearPublish.Text.ToInt();
            bookUpdate.PageNumber = txtPageNumber.Text.ToInt();
            bookUpdate.Size = txtSize.Text;
            bookUpdate.Price = txtPrice.Text.ToDecimal();
            bookUpdate.Authors = listAuthor;

            BookDAL.Instance.Update(bookUpdate, txtNumber.Text.ToInt());
            p.Close();
         });

         RetypeCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) =>
         {
         });

         CancelCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) => { p.Close(); });
      }

      private ObservableCollection<BookCategoryDTO> listBookCategory;
      private ObservableCollection<PublisherDTO> listPublisher;
      private ObservableCollection<Author> listAuthor;
      private ObservableCollection<Author> listBookAuthor;
      private BookCategoryDTO bookCategorySelected;
      private PublisherDTO publisherSelected;
      private Author authorSelected;
      private Author bookAuthorSelected;
   }
}