import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'word-audio-player',
  templateUrl: './word-audio-player.component.html',
  styleUrls: ['./word-audio-player.component.css']
})
export class WordAudioPlayerComponent implements OnInit {

  @Input() word: string;
  @Input() autoplay = false;

  audioUrl: string;

  constructor() { }

  ngOnInit(): void {
    this.audioUrl = "https://vk-word-audio.s3.amazonaws.com/" + this.word.trim().replace(" ", "-").toLowerCase() + ".mp3";
  }

}
