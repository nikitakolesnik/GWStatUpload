import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  readonly api: string = "https://localhost:7000/stats";

  public match_entries: MatchEntry[];

  title = 'WebClient test';

  public offset: number = 0;
  readonly PAGE_SIZE: number = 10;
  constructor(http: HttpClient) {
    http.get<MatchEntry[]>(this.api + '/get_match_page?offset=0&pageSize=' + this.PAGE_SIZE).subscribe(result => {
      this.match_entries = result;
    }, error => console.error(error));
    this.offset += this.PAGE_SIZE;
  }

  calculate_time_diff_string(timestamp: string): string {
    var now = new Date().getTime();
    var then = new Date(timestamp).getTime();
    var diff = now - then;

    var days = Math.floor(diff / (60 * 60 * 24 * 1000));
    var hours = Math.floor(diff / (60 * 60 * 1000)) - (days * 24);
    var minutes = Math.floor(diff / (60 * 1000)) - ((days * 24 * 60) + (hours * 60));
    var seconds = Math.floor(diff / 1000) - ((days * 24 * 60 * 60) + (hours * 60 * 60) + (minutes * 60));

    return 'Submitted ' + days + ' days, ' + hours + ' hours, ' + minutes + ' minutes, ' + seconds + ' seconds ago.';
  }

  get_next_page(http: HttpClient): void {
    //untested
    http.get<MatchEntry[]>(this.api + '/get_match_page?offset=' + this.offset + '&pageSize=' + this.PAGE_SIZE).subscribe(result => {
      console.log(result);
      this.match_entries = this.match_entries.concat(result);
    }, error => console.error(error));
    this.offset += this.PAGE_SIZE;
  }

  public search_query: string;
  public is_search: boolean;
  search_player(): void {
    // remember to either reset offset & page size here, or display a separate filtered list
  }
}

interface MatchEntry {
  submitted: string;
  players: MatchPlayer[];
}
interface MatchPlayer {
  name: string;
  skills: MatchPlayerSkill[];
}
interface MatchPlayerSkill {
  id: number;
  count: number;
}
