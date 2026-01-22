import { Book } from "./book.model";  // 🔧 Changed from "./book" to "./book.model"

export interface BookCategory {
  bookCategoryID: number;
  name: string;
  books?: Book[];  // 🔧 Added ? to make it optional
}
