import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Book } from '../Models/book.model';
import { BookCategory } from '../Models/book-category';

@Injectable({
  providedIn: 'root'
})
export class BooksService {
  private baseUrl: string = "https://localhost:7084";  // ✅ Fixed - removed /Books
  private categoryUrl: string = `${this.baseUrl}/BookCategories`;
  private booksUrl: string = `${this.baseUrl}/Books`;
  private uploadUrl: string = `${this.baseUrl}/FileUpload`;

  constructor(private http: HttpClient) { }

  // ========== Category Related Methods ==========
  getAllBooksWithCategory(): Observable<BookCategory[]> {
    return this.http.get<BookCategory[]>(this.categoryUrl);
  }

  deleteBookWithCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${this.categoryUrl}/${id}`);  // ✅ Fixed - backtick to parentheses
  }

  getCategoryAndBookById(cateId: number): Observable<BookCategory> {
    return this.http.get<BookCategory>(`${this.categoryUrl}/${cateId}`);  // ✅ Fixed
  }

  updateBookAndCategory(cateId: number, category: BookCategory): Observable<BookCategory> {
    return this.http.put<BookCategory>(`${this.categoryUrl}/${cateId}`, category);  // ✅ Fixed
  }

  addBookAndCategory(category: BookCategory): Observable<BookCategory> {
    return this.http.post<BookCategory>(this.categoryUrl, category);
  }

  // ========== Books CRUD Methods ==========
  getAllBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.booksUrl);
  }

  getBookById(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.booksUrl}/${id}`);  // ✅ Fixed
  }

  getBooksByCategory(categoryId: number): Observable<Book[]> {
    return this.http.get<Book[]>(`${this.booksUrl}/Category/${categoryId}`);  // ✅ Fixed
  }

  addBook(book: Book): Observable<Book> {
    return this.http.post<Book>(this.booksUrl, book);
  }

  updateBook(id: number, book: Book): Observable<void> {
    return this.http.put<void>(`${this.booksUrl}/${id}`, book);  // ✅ Fixed
  }

  deleteBook(id: number): Observable<void> {
    return this.http.delete<void>(`${this.booksUrl}/${id}`);  // ✅ Fixed
  }

  updateStock(id: number, stock: number): Observable<void> {
    return this.http.patch<void>(`${this.booksUrl}/${id}/Stock`, stock);  // ✅ Fixed
  }

  // ========== 🆕 Image Upload Methods ==========
  uploadImage(file: File): Observable<{ imageUrl: string }> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<{ imageUrl: string }>(`${this.uploadUrl}/upload`, formData);  // ✅ Fixed
  }

  deleteImage(imageUrl: string): Observable<{ message: string }> {
    return this.http.delete<{ message: string }>(`${this.uploadUrl}/delete?imageUrl=${imageUrl}`);  // ✅ Fixed
  }
}
