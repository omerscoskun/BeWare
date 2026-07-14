import { Component, input } from '@angular/core';

@Component({
  selector: 'app-movie-grid-skeleton',
  template: `
    <div class="grid grid-cols-2 gap-4 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5">
      @for (i of placeholders(); track i) {
        <div class="overflow-hidden rounded-lg bg-neutral-900 ring-1 ring-neutral-800">
          <div class="aspect-[2/3] animate-pulse bg-neutral-800"></div>
          <div class="p-2">
            <div class="h-4 w-3/4 animate-pulse rounded bg-neutral-800"></div>
          </div>
        </div>
      }
    </div>
  `,
})
export class MovieGridSkeleton {
  readonly count = input(10);
  protected placeholders(): number[] {
    return Array.from({ length: this.count() }, (_, i) => i);
  }
}
