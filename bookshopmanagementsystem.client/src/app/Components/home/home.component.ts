import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  // Statistics variables
  totalCategories: number = 4;
  totalBooks: number = 5;
  totalStock: number = 165;
  totalValue: string = '12,650';

  // Categories array (optional - for dynamic rendering)
  categories = [
    {
      title: 'Medical Books',
      subtitle: 'All medical science related books',
      icon: 'bi-heart-pulse-fill',
      class: ''
    },
    {
      title: 'Engineering Books',
      subtitle: 'Engineering and technology related books',
      icon: 'bi-building',
      class: 'success'
    },
    {
      title: 'Stationery',
      subtitle: 'Writing and art supplies',
      icon: 'bi-palette-fill',
      class: 'warning'
    },
    {
      title: 'General Knowledge',
      subtitle: 'General knowledge and information',
      icon: 'bi-book',
      class: 'info'
    },
    {
      title: 'Programming',
      subtitle: 'Computer programming books',
      icon: 'bi-bag-fill',
      class: 'danger'
    },
    {
      title: 'Product Collection',
      subtitle: 'Various product collections',
      icon: 'bi-tools',
      class: 'purple'
    }
  ];

  constructor() { }

  ngOnInit(): void {
    // Component initialization
    this.loadStatistics();
  }

  loadStatistics(): void {
    // Load statistics from a service (example)
    // this.bookService.getStatistics().subscribe(data => {
    //   this.totalCategories = data.categories;
    //   this.totalBooks = data.books;
    //   this.totalStock = data.stock;
    //   this.totalValue = data.value;
    // });

    console.log('Statistics loaded');
  }

  navigateToBooks(): void {
    // Navigation logic if needed
    console.log('Navigating to books page...');
  }

  searchBooks(query: string): void {
    // Search functionality
    console.log('Searching for:', query);
  }

  onCategoryClick(category: any): void {
    // Handle category click
    console.log('Category clicked:', category.title);
  }
}
