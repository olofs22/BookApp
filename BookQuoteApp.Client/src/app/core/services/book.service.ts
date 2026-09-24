import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

export interface Book {
    id: number;
    title: string;
    authorName: string;
    publisherName: string | null;
    publisherDate: string;
}