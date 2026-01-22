import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { BookCategory } from '../../Models/book-category';
import { BooksService } from '../../Services/books.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-view-books',
  standalone: true,
  imports: [CommonModule, RouterLink, HttpClientModule],
  templateUrl: './view-books.component.html',
  styleUrls: ['./view-books.component.css']
})
export class ViewBooksComponent implements OnInit {
  bookList: BookCategory[] = [];
  baseImageUrl: string = 'https://localhost:7084'; // Base URL for images

  constructor(private service: BooksService, private router: Router) { }

  ngOnInit(): void {
    this.getList();
  }

  getList(): void {
    this.service.getAllBooksWithCategory().subscribe({
      next: (res) => {
        this.bookList = res;
        console.log('Books loaded successfully:', res);
      },
      error: (error) => {
        console.error('Error loading books:', error);
        alert('Failed to load books. Please check console for details.');
      }
    });
  }

  deleteItem(category: BookCategory): void {
    const isConfirm = confirm(`Are you sure you want to delete this category: ${category.name}?`);
    if (isConfirm) {
      this.service.deleteBookWithCategory(category.bookCategoryID).subscribe({
        next: () => {
          alert('Successfully deleted');
          this.getList();
        },
        error: (error) => {
          console.error('Error deleting book:', error);
          alert('Failed to delete. Please try again.');
        }
      });
    }
  }

  // ✅ Fixed Get full image URL
  getImageUrl(imageUrl: string | undefined): string {
    return imageUrl
      ? (imageUrl.startsWith('http') ? imageUrl : `${this.baseImageUrl}${imageUrl}`)
      : 'https://via.placeholder.com/150x200/e0e0e0/666666?text=No+Image';
  }
}
