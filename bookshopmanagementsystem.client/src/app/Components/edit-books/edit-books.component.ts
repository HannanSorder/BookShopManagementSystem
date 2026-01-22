import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { Book } from '../../Models/book.model';
import { BookCategory } from '../../Models/book-category';
import { BooksService } from '../../Services/books.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-edit-books',
  standalone: true,
  imports: [FormsModule, CommonModule, HttpClientModule, RouterModule],
  templateUrl: './edit-books.component.html',
  styleUrls: ['./edit-books.component.css']
})
export class EditBooksComponent implements OnInit {
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

  constructor(
    private service: BooksService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if (id) {
          this.service.getCategoryAndBookById(Number(id)).subscribe({
            next: (res) => {
              this.bookList = res.books || [];
              this.bookCategory = {
                bookCategoryID: res.bookCategoryID,
                name: res.name,
                books: this.bookList
              };
            },
            error: (err) => {
              alert('Failed to load category: ' + err.message);
            }
          });
        } else {
          alert('Invalid category ID');
        }
      }
    });
  }

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

  // 🆕 Upload image before saving book
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
    if (!this.bookObj.title || !this.bookObj.isbn || !this.bookObj.author) {
      alert('Please fill in Title, ISBN, and Author');
      return;
    }
    try {
      if (this.selectedFile) {
        this.bookObj.imageUrl = await this.uploadImageIfSelected();
      }
      this.bookObj.bookCategoryID = this.bookCategory.bookCategoryID;
      this.bookList.unshift({ ...this.bookObj });

      // Reset form
      this.bookObj = {
        bookID: 0, title: '', isbn: '', author: '', publisher: '',
        purchasePrice: 0, sellingPrice: 0, pages: 0, stock: 0,
        bookCategoryID: this.bookCategory.bookCategoryID, imageUrl: ''
      };
      this.selectedFile = null;
      this.imagePreview = null;
      const fileInput = document.getElementById('bookImage') as HTMLInputElement;
      if (fileInput) fileInput.value = '';
    } catch (error) {
      console.error('Error adding book:', error);
    }
  }

  deleteBook(b: Book) {
    const isConfirm = confirm(`Delete book: ${b.title}?`);
    if (isConfirm) {
      const index = this.bookList.findIndex(obj => obj.title === b.title && obj.isbn === b.isbn);
      if (index > -1) this.bookList.splice(index, 1);
    }
  }

  updateBookCategory() {
    if (!this.bookCategory.name) {
      alert('Please enter the category name');
      return;
    }
    this.bookCategory.books = this.bookList;
    this.service.updateBookAndCategory(this.bookCategory.bookCategoryID, this.bookCategory).subscribe({
      next: () => {
        alert('Successfully updated');
        this.router.navigate(['/books']);
      },
      error: err => {
        alert('An error occurred: ' + err.message);
      }
    });
  }

  cancel() {
    this.router.navigate(['/books']);
  }

  // ✅ Fixed getImageUrl method
  getImageUrl(imageUrl: string | undefined): string {
    return imageUrl
      ? (imageUrl.startsWith('http') ? imageUrl : `https://localhost:7084${imageUrl}`)
      : 'https://via.placeholder.com/150x200/e0e0e0/666666?text=No+Image';
  }
}
