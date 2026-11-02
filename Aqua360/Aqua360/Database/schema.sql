CREATE DATABASE IF NOT EXISTS aqua360
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE aqua360;

CREATE TABLE IF NOT EXISTS customers
(
	id INT NOT NULL AUTO_INCREMENT,
	account_number VARCHAR(32) NOT NULL,
	full_name VARCHAR(160) NOT NULL,
	identification VARCHAR(32) NOT NULL,
	address VARCHAR(255) NOT NULL,
	phone VARCHAR(32) NOT NULL,
	email VARCHAR(254) NULL,
	is_active TINYINT(1) NOT NULL DEFAULT 1,
	created_at DATETIME(6) NOT NULL DEFAULT (UTC_TIMESTAMP(6)),
	updated_at DATETIME(6) NULL,
	CONSTRAINT pk_customers PRIMARY KEY (id),
	CONSTRAINT uq_customers_account_number UNIQUE (account_number),
	INDEX ix_customers_full_name (full_name),
	INDEX ix_customers_is_active (is_active)
) ENGINE = InnoDB
  DEFAULT CHARACTER SET = utf8mb4
  COLLATE = utf8mb4_unicode_ci;
