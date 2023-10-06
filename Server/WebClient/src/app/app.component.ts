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

  get_next_page(http: HttpClient): void {
    //untested
    http.get<MatchEntry[]>(this.api + '/get_match_page?offset=' + this.offset + '&pageSize=' + this.PAGE_SIZE).subscribe(result => {
      console.log(result);
      this.match_entries = this.match_entries.concat(this.match_entries, result);
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
