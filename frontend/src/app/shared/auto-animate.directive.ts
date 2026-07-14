import { Directive, ElementRef, OnInit, inject } from '@angular/core';
import autoAnimate from '@formkit/auto-animate';

@Directive({
  selector: '[appAutoAnimate]',
})
export class AutoAnimate implements OnInit {
  private readonly host = inject<ElementRef<HTMLElement>>(ElementRef);

  ngOnInit(): void {
    autoAnimate(this.host.nativeElement);
  }
}
