using System;

namespace LibraryManager.EntityFramework.Model
{
   public class BorrowDTO : Borrow
   {
      public DateTime TermDate { get  => this.BorrowDate.AddDays((double)this.Book.BookCategory.LimitDays); } 
      public string LibrarianName { get => this.Librarian.LastName + " " + this.Librarian.FirstName; } 
      public string MemberName { get => this.Member.LastName + " " + this.Member.FirstName; } 

      public BorrowDTO() : base()
      {
      }

      public BorrowDTO(Borrow borrowRaw) : base()
      {
         if (borrowRaw != null)
         {
            this.Id = borrowRaw.Id;
            this.Book = borrowRaw.Book;
            this.BookId = borrowRaw.BookId;
            this.BorrowDate = borrowRaw.BorrowDate;
            this.Librarian = borrowRaw.Librarian;
            this.LibrarianId = borrowRaw.LibrarianId;
            this.Member = borrowRaw.Member;
            this.MemberId = borrowRaw.MemberId;
         }
      }
   }
}