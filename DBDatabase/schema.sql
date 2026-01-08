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

CREATE TABLE contributors (
    cont_id UUID PRIMARY KEY,
    cont_username NVARCHAR(50),
    cont_nickname NVARCHAR(50),
    cont_x_link TEXT,
    cont_image_url TEXT,
    cont_note TEXT,
    cont_wallet_address TEXT,
    cont_created_at TIMESTAMP,
    cont_soft_deleted BOOLEAN,
);

