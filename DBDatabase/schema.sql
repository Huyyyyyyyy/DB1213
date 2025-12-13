CREATE TABLE medias (
    med_id UUID PRIMARY KEY,
    med_file_type VARCHAR(50),
    med_content TEXT,
    med_created_at TIMESTAMP,
    med_path TEXT
);
