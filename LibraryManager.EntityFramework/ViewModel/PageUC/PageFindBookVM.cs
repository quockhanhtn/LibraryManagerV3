using LibraryManager.EntityFramework.Model;
using LibraryManager.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManager.EntityFramework.ViewModel
{
   public class PageFindBookVM : BaseViewModel
   {
      public ObservableCollection<BookDTO> ListBook { get => listBook; set { listBook = value; OnPropertyChanged(); } }
      public ObservableCollection<BookCategoryDTO> ListBookCategory { get => listBookCategory; set { listBookCategory = value; OnPropertyChanged(); } }
      public ObservableCollection<PublisherDTO> ListPublisher { get => listPublisher; set { listPublisher = value; OnPropertyChanged(); } }
      public BookDTO BookSelected { get => bookSelected; set { bookSelected = value; OnPropertyChanged(); } }
      public BookCategoryDTO BookCategorySelected { get => bookCategorySelected; set { bookCategorySelected = value; OnPropertyChanged(); ReloadList(); } }
      public PublisherDTO PublisherSelected { get => publisherSelected; set { publisherSelected = value; OnPropertyChanged(); ReloadList(); } }

      public ICommand SearchCommand { get; set; }
      public ICommand ChangeBookCategoryCommand { get; set; }
      public ICommand ChangePublisherCommand { get; set; }
      public ICommand BookSelectedChanged { get; set; }

      public ICommand ObjectSelectedChangedCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public ICommand StatusChangeCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public ICommand DeleteCommand { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      public PageFindBookVM()
      {
         ListBookCategory = BookCategoryDAL.Instance.GetList(EStatusFillter.Active);
         ListBookCategory.Add(new BookCategoryDTO() { Id = 0, Name = "Tất cả chuyên mục" });
         ListPublisher = PublisherDAL.Instance.GetList(EStatusFillter.Active);
         ListPublisher.Add(new PublisherDTO() { Id = 0, Name = "Tất cả NXB" });
         ReloadList();

         SearchCommand = new RelayCommand<TextBox>((p) => { return p != null; }, (p) =>
         {
            if (p.Text == "" || p.Text == " ")
            {
               p.Text = "";
               ReloadList();
               return;
            }

            var searchKeyWord = p.Text.RemoveUnicode().ToLower();

            ReloadList();
            ListBook = new ObservableCollection<BookDTO>(ListBook.Where(
                   x => x.Title.RemoveUnicode().ToLower().Contains(searchKeyWord)
                   || x.AuthorNames.RemoveUnicode().ToLower().Contains(searchKeyWord)));
         });
      }

      public void ReloadList()
      {
         if (BookCategorySelected == null && PublisherSelected == null)
         {
            ListBook = BookDAL.Instance.GetList();
         }
         else if (BookCategorySelected == null)
         {
            ListBook = BookDAL.Instance.GetList(0, PublisherSelected.Id);
         }
         else if (PublisherSelected == null)
         {
            ListBook = BookDAL.Instance.GetList(BookCategorySelected.Id, 0);
         }
         else { ListBook = BookDAL.Instance.GetList(BookCategorySelected.Id, PublisherSelected.Id); }
      }

      private ObservableCollection<BookDTO> listBook;
      private ObservableCollection<BookCategoryDTO> listBookCategory;
      private ObservableCollection<PublisherDTO> listPublisher;
      private BookDTO bookSelected;
      private BookCategoryDTO bookCategorySelected;
      private PublisherDTO publisherSelected;
   }
}