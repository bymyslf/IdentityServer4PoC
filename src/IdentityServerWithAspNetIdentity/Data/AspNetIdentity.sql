CREATE TABLE asp_net_roles (
  id VARCHAR(128) NOT NULL,
  name TEXT NOT NULL,
  concurrency_stamp TEXT,
  CONSTRAINT asp_net_roles_pkey PRIMARY KEY(id)
) 
WITH (oids = false);

CREATE TABLE asp_net_users (
  id TEXT NOT NULL,
  user_name TEXT NOT NULL,
  normalized_user_name TEXT NOT NULL,
  password_hash TEXT,
  security_stamp TEXT,
  email TEXT,
  normalized_email TEXT,
  email_confirmed BOOLEAN DEFAULT false NOT NULL,
  phone_number TEXT,
  normalized_phone_number TEXT,
  phone_number_confirmed BOOLEAN DEFAULT false NOT NULL,
  two_factor_enabled BOOLEAN DEFAULT false NOT NULL,
  lockout_end TIMESTAMP WITH TIME ZONE,
  lockout_enabled BOOLEAN DEFAULT false NOT NULL,
  access_failed_count INTEGER NOT NULL,
  concurrency_stamp TEXT,
  CONSTRAINT asp_net_users_pkey PRIMARY KEY(id)
) 
WITH (oids = false);


CREATE TABLE asp_net_user_claims (
  id SERIAL,
  claim_type TEXT,
  claim_value TEXT,
  user_id TEXT NOT NULL,
  CONSTRAINT asp_net_user_claims_pkey PRIMARY KEY(id),
  CONSTRAINT fk_asp_net_user_claims_asp_net_users_user_id FOREIGN KEY (user_id)
    REFERENCES asp_net_users(id)
    ON DELETE CASCADE
    ON UPDATE NO ACTION
    NOT DEFERRABLE
) 
WITH (oids = false);


CREATE TABLE asp_net_user_logins (
  user_id TEXT NOT NULL,
  login_provider TEXT NOT NULL,
  provider_key TEXT NOT NULL,
  provider_display_name TEXT,
  CONSTRAINT asp_net_user_logins_pkey PRIMARY KEY(user_id, login_provider, provider_key),
  CONSTRAINT fk_asp_net_user_logins_asp_net_users_user_id FOREIGN KEY (user_id)
    REFERENCES asp_net_users(id)
    ON DELETE CASCADE
    ON UPDATE NO ACTION
    NOT DEFERRABLE
) 
WITH (oids = false);


CREATE TABLE asp_net_user_roles (
  user_id TEXT NOT NULL,
  role_id VARCHAR(128) NOT NULL,
  CONSTRAINT asp_net_user_roles_pkey PRIMARY KEY(user_id, role_id),
  CONSTRAINT fk_asp_net_user_roles_asp_net_roles_role_id FOREIGN KEY (role_id)
    REFERENCES asp_net_roles(id)
    ON DELETE CASCADE
    ON UPDATE NO ACTION
    NOT DEFERRABLE,
  CONSTRAINT fk_asp_net_user_roles_asp_net_users_user_id FOREIGN KEY (user_id)
    REFERENCES asp_net_users(id)
    ON DELETE CASCADE
    ON UPDATE NO ACTION
    NOT DEFERRABLE
) 
WITH (oids = false);

CREATE INDEX ix_asp_net_user_roles_role_id ON asp_net_user_roles
  USING btree (role_id COLLATE pg_catalog."default");

CREATE INDEX ix_asp_net_user_roles_user_id ON asp_net_user_roles
  USING btree (user_id);

CREATE INDEX ix_asp_net_user_claims_user_id	ON asp_net_user_claims	(user_id);
CREATE INDEX ix_asp_net_user_logins_user_id	ON asp_net_user_logins	(user_id);