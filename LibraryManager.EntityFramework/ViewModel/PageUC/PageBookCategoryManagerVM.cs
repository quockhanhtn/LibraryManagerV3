﻿using LibraryManager.EntityFramework.Model;
using LibraryManager.EntityFramework.View;
using LibraryManager.MyUserControl.MyBox;
using LibraryManager.Utils;
using MaterialDesignThemes.Wpf;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManager.EntityFramework.ViewModel
{
   public class PageBookCategoryManagerVM : BaseViewModel, IObjectManager, ICopyInfo
   {
      public bool IsShowDeleteCategory { get => isShowDeleteCategory; set { isShowDeleteCategory = value; ReloadList(); } }

      public ObservableCollection<BookCategoryDTO> ListBookCategory { get => listBookCategory; set { listBookCategory = value; OnPropertyChanged(); } }
      public BookCategoryDTO BookCategorySelected { get => bookCategorySelected; set { bookCategorySelected = value; OnPropertyChanged(); } }
      public ICommand SearchCommand { get; set; }
      public ICommand ExportToExcelCommand { get; set; }
      public ICommand ObjectSelectedChangedCommand { get; set; }
      public ICommand AddCommand { get; set; }
      public ICommand UpdateCommand { get; set; }
      public ICommand StatusChangeCommand { get; set; }
      public ICommand DeleteCommand { get; set; }
      public ICommand CopyIdCommand { get; set; }
      public ICommand CopyNameCommand { get; set; }

      public PageBookCategoryManagerVM()
      {
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

            ListBookCategory = new ObservableCollection<BookCategoryDTO>(BookCategoryDAL.Instance.GetList().Where(
                   x => x.Name.RemoveUnicode().ToLower().Contains(searchKeyWord)));
         });

         ObjectSelectedChangedCommand = new RelayCommand<UserControl>((p) => { return p != null && BookCategorySelected != null; }, (p) =>
         {
            var btnStatusChange = p.FindName("btnStatusChange") as Button;
            var mnuStatusChange = p.FindName("mnuStatusChange") as MenuItem;
            //var tblStatusChange = p.FindName("tblStatusChange") as TextBlock;
            //var icoStatusChange = p.FindName("icoStatusChange") as PackIcon;
            if (BookCategorySelected.Status == true)
            {
               //tblStatusChange.Text = "THÔI VIỆC";
               //icoStatusChange.Kind = PackIconKind.BlockHelper;
               btnStatusChange.Content = "ẨN";
               btnStatusChange.ToolTip = "Ẩn chuyên mục \"" + BookCategorySelected.Name + "\"";
               mnuStatusChange.Header = "Ẩn chuyên mục";
            }
            else
            {
               //tblStatusChange.Text = "ĐI LÀM LẠI";
               //icoStatusChange.Kind = PackIconKind.Restore;
               btnStatusChange.Content = "HIỂN THỊ";
               btnStatusChange.ToolTip = "Hiển thị chuyên mục \"" + BookCategorySelected.Name + "\"";
               mnuStatusChange.Header = "Hiển thị chuyên mục";
            }
         });

         ExportToExcelCommand = new RelayCommand<object>((p) => true, (p) =>
         {
            string filePath = DialogUtils.ShowSaveFileDialog("Xuất danh chuyên mục sách", "Excel | *.xlsx | Excel 2003 | *.xls");
            if (string.IsNullOrEmpty(filePath)) { return; }

            try
            {
               ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
               using (var excelPackage = new ExcelPackage())
               {
                  ExcelHelper.SetExcelPackageInfo(excelPackage, "Library Manger", "Danh sách chuyên mục sách", new List<string>() { "List BookCategory Sheet" });

                  // lấy sheet vừa add ra để thao tác
                  ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

                  ExcelHelper.SetSheetInfo(worksheet, "List BookCategory Sheet");

                  // set column width
                  ExcelHelper.SetColumWidth(worksheet, new double[] { 30, 30, 30, 30, 30 });

                  // Tạo danh sách các column header
                  string[] arrColumnHeader = { "Mã chuyên mục", "Tên chuyên mục", "Số ngày cho mượn", "Số lượng sách", "Ghi chú" };

                  // lấy ra số lượng cột cần dùng dựa vào số lượng header
                  var countColHeader = arrColumnHeader.Count();

                  // merge các column lại từ column 1 đến số column header
                  // gán giá trị cho cell vừa merge là Thống kê thông tni User Kteam
                  worksheet.Cells[1, 1].Value = "Thống kê chuyên mục sách".ToUpper();
                  ExcelHelper.MergeAndCenter(worksheet, 1, 1, 1, countColHeader, true);

                  worksheet.Cells[2, 1].Value = "Ngày " + DateTime.Now.ToString();
                  ExcelHelper.MergeAndCenter(worksheet, 2, 1, 2, countColHeader, true);

                  int colIndex = 1, rowIndex = 3;

                  //tạo các header từ column header đã tạo từ bên trên
                  foreach (var item in arrColumnHeader)
                  {
                     var cell = worksheet.Cells[rowIndex, colIndex];

                     //set màu thành gray
                     var fill = cell.Style.Fill;
                     fill.PatternType = ExcelFillStyle.Solid;
                     fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                     //căn chỉnh các border
                     var border = cell.Style.Border;
                     border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     //gán giá trị
                     cell.Value = item;

                     colIndex++;
                  }

                  // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                  foreach (var item in ListBookCategory)
                  {
                     colIndex = 1;   // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                     rowIndex++;     // rowIndex tương ứng từng dòng dữ liệu

                     //gán giá trị cho từng cell
                     ExcelHelper.FormatCellBorder(worksheet, rowIndex, colIndex);
                     worksheet.Cells[rowIndex, colIndex++].Value = item.Id;

                     ExcelHelper.FormatCellBorder(worksheet, rowIndex, colIndex);
                     worksheet.Cells[rowIndex, colIndex++].Value = item.Name;

                     ExcelHelper.FormatCellBorder(worksheet, rowIndex, colIndex);
                     worksheet.Cells[rowIndex, colIndex++].Value = item.LimitDays;

                     ExcelHelper.FormatCellBorder(worksheet, rowIndex, colIndex);
                     worksheet.Cells[rowIndex, colIndex++].Value = item.NumberOfBook;

                     ExcelHelper.FormatCellBorder(worksheet, rowIndex, colIndex);
                     worksheet.Cells[rowIndex, colIndex++].Value = item.Note;
                  }

                  ExcelHelper.SaveExcelPackage(excelPackage, filePath);
               }

               if (MyMessageBox.Show("Bạn có muốn mở file excel vừa xuất không ?", "Xuất file Excel thành công !", "Có", "Không", MessageBoxImage.Information))
               {
                  ExcelHelper.OpenFile(filePath);
               }
            }
            catch (Exception)
            {
               MyMessageBox.Show("Có lỗi khi lưu file!", "Thông báo", "OK", "", MessageBoxImage.Error);
            }
         });

         AddCommand = new RelayCommand<UserControl>((p) => { return p != null; }, (p) =>
         {
            var addDataContext = new AddBookCategoryWindowVM();
            var addBookCategoryWindow = new AddBookCategoryWindow() { DataContext = addDataContext };
            addBookCategoryWindow.ShowDialog();

            var mySnackbar = p.FindName("mySnackbar") as Snackbar;
            if (addDataContext.Result != null)
            {
               mySnackbar.MessageQueue.Enqueue("Thêm chuyên mục \"" + addDataContext.Result.Name + "\" thành công");
               ReloadList();
            }
            else { mySnackbar.MessageQueue.Enqueue("Không có thay đổi"); }
         });

         UpdateCommand = new RelayCommand<UserControl>((p) => { return p != null && BookCategorySelected != null; }, (p) =>
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

            BookCategorySelected.Name = txtName.Text;
            BookCategorySelected.LimitDays = txtLimitDays.Text.ToInt();

            BookCategoryDAL.Instance.Update(BookCategorySelected);
            var mySnackbar = p.FindName("mySnackbar") as Snackbar;
            mySnackbar.MessageQueue.Enqueue("Cập nhật chuyên mục \"" + BookCategorySelected.Name + "\" thành công");
            ReloadList();
         });

         StatusChangeCommand = new RelayCommand<object>((p) => { return BookCategorySelected != null; }, (p) =>
         {
            BookCategoryDAL.Instance.ChangeStatus(BookCategorySelected.Id);
            ReloadList();
         });

         DeleteCommand = new RelayCommand<UserControl>((p) => { return BookCategorySelected != null && BookCategorySelected.NumberOfBook == 0; }, (p) =>
         {
            BookCategoryDAL.Instance.Delete(BookCategorySelected.Id);
            ReloadList();
            var mySnackbar = p.FindName("mySnackbar") as Snackbar;
            mySnackbar.MessageQueue.Enqueue("Xóa chuyên mục thành công !");
         });

         CopyIdCommand = new RelayCommand<object>((p) => { return BookCategorySelected != null; }, (p) => { Clipboard.SetText(BookCategorySelected.Id.ToString()); });

         CopyNameCommand = new RelayCommand<object>((p) => { return BookCategorySelected != null; }, (p) => { Clipboard.SetText(BookCategorySelected.Name); });
      }

      private void ReloadList()
      {
         if (isShowDeleteCategory) { ListBookCategory = BookCategoryDAL.Instance.GetList(); }
         else { ListBookCategory = BookCategoryDAL.Instance.GetList(EStatusFillter.Active); }
      }

      private ObservableCollection<BookCategoryDTO> listBookCategory;
      private BookCategoryDTO bookCategorySelected;

      private bool isShowDeleteCategory = false;
   }
}