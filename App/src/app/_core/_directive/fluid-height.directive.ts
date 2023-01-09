import { DataService } from 'src/app/_core/_service/data.service';
import {
  AfterViewInit,
  Directive,
  ElementRef,
  HostBinding,
  Input,
  OnDestroy,
  OnInit,
  Renderer2,
} from '@angular/core';
import { fromEvent, Subject, Subscription } from 'rxjs';
import { debounceTime, throttleTime } from 'rxjs/operators';
import { Custom } from '../_model/contribution';


@Directive({
  selector: '[fluidHeight],[fluidHeight2]',
  outputs: [
		"fluidHeight",
		"fluidHeight2"
	]
})
export class FluidHeightDirective implements AfterViewInit, OnDestroy , OnInit {
  @Input()
  fluidHeight: string;
  data: Array<Custom> = [];
  subscription: Subscription[] = [];
  subject = new Subject<string>();
  constructor(
    private renderer: Renderer2,
    private dataService: DataService,
    private elementRef: ElementRef
    ) {

  }

  ngAfterViewInit() {
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.contentHeight(this.elementRef.nativeElement, this.fluidHeight);
    }, 500);
  }


  ngOnDestroy(){
    this.subscription.forEach(item => item.unsubscribe());
  }

  contentHeight(parent: HTMLElement, className) {
    var height = parent.offsetHeight;
    var month = className.month;
    var actionId = className.actionId;
    var data = new Custom();
    data.value = height;
    data.month = month;
    data.actionId = actionId;
    this.dataService.changeMessage(data)

  }

}
