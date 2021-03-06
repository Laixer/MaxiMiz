﻿using Laixer.Identity.Dapper;
using System;

namespace Maximiz.Identity
{

    /// <summary>
    /// Contains custom query strings for the identity framework.
    /// TODO Look at these, they were just copied from FunderMaps atm.
    /// </summary>
    internal class CustomQueryRepository : ICustomQueryRepository
    {

        /// <summary>
        /// Configure custom queries.
        /// </summary>
        /// <param name="queryRepository">Existing query repository.</param>
        public void Configure(IQueryRepository queryRepository)
        {
            if (queryRepository == null)
            {
                throw new ArgumentNullException(nameof(queryRepository));
            }

            queryRepository.CreateAsync = $@"
                INSERT INTO application.user
                    (email,
                    normalized_email,
                    email_confirmed,
                    password_hash,
                    security_stamp,
                    phone_number,
                    lockout_end,
                    given_name,
                    last_name)
                VALUES
                    (@Email,
                    @NormalizedEmail,
                    @EmailConfirmed,
                    @PasswordHash,
                    @SecurityStamp,
                    @PhoneNumber,
                    @LockoutEnd,
                    @GivenName,
                    @LastName)
                RETURNING id";

            queryRepository.FindByNameAsync = @"
                SELECT *
                FROM application.user
                WHERE normalized_email=@NormalizedUserName";

            queryRepository.UpdateAsync = @"
                UPDATE application.user
                SET    email = @Email,
                       normalized_email = @NormalizedEmail,
                       email_confirmed = @EmailConfirmed,
                       given_name = @GivenName,
                       last_name = @LastName,
                       avatar = @Avatar,
                       job_title = @JobTitle,
                       password_hash = @PasswordHash,
                       phone_number = @PhoneNumber,
                       phone_number_confirmed = @PhoneNumberConfirmed,
                       two_factor_enabled = @TwoFactorEnabled,
                       lockout_end = @LockoutEnd,
                       lockout_enabled = @LockoutEnabled,
                       access_failed_count = @AccessFailedCount,
                       given_name = @GivenName,
                       last_name = @LastName,
                WHERE  id = @Id";

            queryRepository.FindByIdAsync = $@"
                SELECT *
                FROM application.user
                WHERE Id=@Id::uuid
                LIMIT 1";

            queryRepository.AddToRoleAsync = $@"
                UPDATE application.user
                SET role = @Role
                WHERE id=@Id";

            queryRepository.GetRolesAsync = $@"
                SELECT role
                FROM application.user
                WHERE id=@Id";

            queryRepository.GetUsersInRoleAsync = $@"
                SELECT *
                FROM application.user
                WHERE role=@Role";

            queryRepository.RemoveFromRoleAsync = $@"
                UPDATE application.user
                SET role = 'user'
                WHERE id=@Id";
        }
    }

}

