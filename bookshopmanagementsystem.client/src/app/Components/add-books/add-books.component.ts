import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Book } from '../../Models/book.model';
import { BookCategory } from '../../Models/book-category';
import { BooksService } from '../../Services/books.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-add-books',
  standalone: true,
  imports: [FormsModule, CommonModule, HttpClientModule],
  templateUrl: './add-books.component.html',
  styleUrls: ['./add-books.component.css']
})
export class AddBooksComponent {
  bookObj: Book = {
    bookID: 0, title: '', isbn: '', author: '', publisher: '',
    purchasePrice: 0, sellingPrice: 0, pages: 0, stock: 0, 
    bookCategoryID: 0, imageUrl: '' 
  };

  bookList: Book[] = [];
  bookCategory: BookCategory = { name: '', bookCategoryID: 0, books: [] };

  // 🆕 Image upload properties
  selectedFile: File | null = null;
  imagePreview: string | null = null;
  isUploading: boolean = false;

  constructor(private service: BooksService, private router: Router) { }

  // 🆕 Handle file selection
  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/webp'];
      if (!allowedTypes.includes(file.type)) {
        alert('Invalid file type. Only images are allowed (JPG, PNG, GIF, WEBP)');
        return;
      }
      if (file.size > 5 * 1024 * 1024) {
        alert('File size must be less than 5MB');
        return;
      }
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = (e: any) => { this.imagePreview = e.target.result; };
      reader.readAsDataURL(file);
    }
  }

  // 🆕 Upload image before adding book
  uploadImageIfSelected(): Promise<string> {
    return new Promise((resolve, reject) => {
      if (this.selectedFile) {
        this.isUploading = true;
        this.service.uploadImage(this.selectedFile).subscribe({
          next: (response) => {
            this.isUploading = false;
            resolve(response.imageUrl);
          },
          error: (err) => {
            this.isUploading = false;
            alert('Failed to upload image. Please try again.');
            reject(err);
          }
        });
      } else {
        resolve('');
      }
    });
  }

  async addBook() {
    if (!this.bookObj.title) {
      alert('Please enter the book title');
      return;
    }
    try {
      if (this.selectedFile) {
        this.bookObj.imageUrl = await this.uploadImageIfSelected();
      }
      this.bookList.unshift({ ...this.bookObj });
      this.bookObj = {
        bookID: 0, title: '', isbn: '', author: '', publisher: '',
        purchasePrice: 0, sellingPrice: 0, pages: 0, stock: 0, 
        bookCategoryID: 0, imageUrl: ''
      };
      this.selectedFile = null;
      this.imagePreview = null;
      const fileInput = document.getElementById('bookImage') as HTMLInputElement;
      if (fileInput) fileInput.value = '';
    } catch (error) {
      console.error('Error adding book:', error);
    }
  }

  deleteBook(b: Book, array: Book[]) {
    const isConfirm = confirm(`Are you sure you want to delete "${b.title}"?`);
    if (isConfirm) {
      const row = array.findIndex(obj => obj.title === b.title && obj.isbn === b.isbn);
      if (row > -1) array.splice(row, 1);
    }
  }

  addBookCategory() {
    if (!this.bookCategory.name) {
      alert('Please enter the category name');
      return;
    }
    if (this.bookList.length === 0) {
      alert('Please add at least one book to the category');
      return;
    }
    const cate: BookCategory = {
      books: this.bookList,
      name: this.bookCategory.name,
      bookCategoryID: 0
    };
    this.service.addBookAndCategory(cate).subscribe({
      next: () => {
        alert("Successfully added");
        this.router.navigate(['books']);
      },
      error: err => {
        alert('An error occurred: ' + err.message);
      }
    });
  }

  // ✅ Fixed getImageUrl method
  getImageUrl(imageUrl: string | undefined): string {
    return imageUrl
      ? (imageUrl.startsWith('http') ? imageUrl : `https://localhost:7084${imageUrl}`)
      : 'https://via.placeholder.com/150x200/e0e0e0/666666?text=No+Image';
  }
}
