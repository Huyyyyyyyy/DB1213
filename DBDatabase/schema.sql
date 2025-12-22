CREATE TABLE medias (
    med_id UUID PRIMARY KEY,
    med_file_type VARCHAR(50),
    med_content TEXT,
    med_created_at TIMESTAMP,
    med_path TEXT
);

CREATE TABLE projects (
    proj_id UUID PRIMARY KEY,
    proj_name VARCHAR(50),
    proj_type VARCHAR(50),
    proj_img TEXT,
    proj_official_site TEXT,
    proj_other_site TEXT,
    proj_created_at TIMESTAMP
);
