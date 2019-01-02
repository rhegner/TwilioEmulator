export class Page<TItem> {
  public currentPage = 1;
  public totalPages = 1;
  public totalItems = 0;
  public itemsPerPage = 0;
  public items: TItem[] = [];
}
