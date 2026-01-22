export interface Book {
  bookID: number;
  title: string;
  isbn: string;
  author: string;
  publisher: string;
  purchasePrice: number;
  sellingPrice: number;
  pages: number;
  stock: number;
  bookCategoryID: number;
  imageUrl?: string;  // 🆕 NEW field
}

export interface BookCategory {
  bookCategoryID: number;
  name: string;
  books?: Book[];
}
