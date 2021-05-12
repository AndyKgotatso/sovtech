export interface SearchResponse {
    chuck: Chuck;
    chuckPager: ChuckPager;
    swapi: Swapi;
}

export interface Result {
    categories: string[];
    created_at: string;
    icon_url: string;
    id: string;
    updated_at: string;
    url: string;
    value: string;
}

export interface Chuck {
    total: number;
    result: Result[];
}

export interface ChuckPager {
    totalItems: number;
    currentPage: number;
    pageSize: number;
    totalPages: number;
    startPage: number;
    endPage: number;
    startIndex: number;
    endIndex: number;
    pages: number[];
}

export interface Result2 {
    name: string;
    height: string;
    mass: string;
    hair_color: string;
    skin_color: string;
    eye_color: string;
    birth_year: string;
    gender: string;
    homeworld: string;
    films: string[];
    species: any[];
    vehicles: any[];
    starships: string[];
    created: Date;
    edited: Date;
    url: string;
}

export interface Swapi {
    count: number;
    next?: any;
    previous?: any;
    results: Result2[];
}

