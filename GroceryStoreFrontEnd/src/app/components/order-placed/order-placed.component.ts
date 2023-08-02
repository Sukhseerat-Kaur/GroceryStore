import { Component, Input, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-order-placed',
  templateUrl: './order-placed.component.html',
  styleUrls: ['./order-placed.component.css'],
})
export class OrderPlacedComponent implements OnInit {
  orderId!: string;
  constructor(private router: Router, private activatedRoute: ActivatedRoute) {}
  ngOnInit(): void {
    this.orderId = this.activatedRoute.snapshot.paramMap.get(
      'orderId'
    ) as string;
  }

  goToDashboard() {
    this.router.navigate(['/dashboard']);
  }
}
